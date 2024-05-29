using FitnessTracker.Auth;
using FitnessTracker.Models;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using ProjectGym.Services.Update;
using ProjectGym.Utilities;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using static ProjectGym.Controllers.UserController;

namespace ProjectGym.Controllers
{
    [Route("api/user")]
    [ApiController]
    public partial class UserController(IReadService<User> readService,
                          IEntityMapper<User, UserDTO> mapper,
                          ICreateService<User> createService,
                          IDeleteService<User> deleteService,
                          IUpdateService<User> updateService,
                          TokenManager tokenManager) : ControllerBase, ICreateController<User, RegisterDTO>
    {
        public IReadService<User> ReadService { get; } = readService;
        public IEntityMapper<User, UserDTO> Mapper { get; } = mapper;
        public ICreateService<User> CreateService { get; } = createService;
        public IDeleteService<User> DeleteService { get; } = deleteService;
        public IUpdateService<User> UpdateService { get; } = updateService;
        public TokenManager TokenManager { get; } = tokenManager;

        private OkObjectResult SetupTokens(User user)
        {
            var (jwt, refresh) = TokenManager.CreateJWTAndRefreshToken(user);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false, //TODO-PROD: Set to true in production
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", refresh, cookieOptions);

            return Ok(jwt);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Create([FromBody] RegisterDTO userDTO)
        {
            if (!ValidEmailRegex().IsMatch(userDTO.Email))
                return BadRequest("Invalid email address");

            if (userDTO.Password.Length < 8)
                return BadRequest("Password must be at least 8 characters long");

            byte[] salt = HashingService.GenerateSalt();
            User user = new()
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                Salt = salt,
                PasswordHash = userDTO.Password.HashPassword(salt),
                Role = Role.User
            };

            var newEntityId = await CreateService.Add(user);
            if (newEntityId == default)
                return BadRequest("User already exists");

            return SetupTokens(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO userDTO)
        {
            try
            {
                //This is just so the server doesn't waste time on checking for an invalid email
                if (!ValidEmailRegex().IsMatch(userDTO.Email) || userDTO.Password.Length < 8)
                    return BadRequest("Incorrect email or password");

                var user = await ReadService.Get(x => x.Email == userDTO.Email, "none");

                var hash = userDTO.Password.HashPassword(user.Salt);
                if (!user.PasswordHash.SequenceEqual(hash))
                    return BadRequest("Incorrect email or password");

                return SetupTokens(user);
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage(false));
            }
        }

        [Authorize]
        [HttpGet("authenticate")]
        public async Task<IActionResult> Authenticate()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity)
                return Unauthorized();

            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim is null)
                return Unauthorized();

            try
            {
                return Ok(Mapper.Map(await ReadService.Get(userIdClaim, "all")));
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage(false));
            }
        }

        [Authorize(AuthenticationSchemes = "AllowExpired")]
        [HttpPut("refresh")]
        public async Task<IActionResult> Refresh()
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity)
                    return Unauthorized();

                if (claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || claimsIdentity.FindFirst(JwtRegisteredClaimNames.Jti)?.Value is not string jwtIdString
                    || !Request.Cookies.TryGetValue("refreshToken", out var refreshTokenString)
                    || !Guid.TryParse(jwtIdString, out var jwtId)
                    || !Guid.TryParse(userIdString, out var userId)
                    || !Guid.TryParse(refreshTokenString, out var refreshToken))
                    return BadRequest("Invalid token");

                var newJwt = await TokenManager.RefreshJWT(jwtId, refreshToken, userId);
                return Ok(newJwt);
            }
            catch (Exception)
            {
                return BadRequest("Invalid token");
            }
        }

        [Authorize]
        [HttpPut("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity)
                return Unauthorized();

            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim is null)
                return Unauthorized();

            try
            {
                var user = await ReadService.Get(userIdClaim, "none");
                if (!user.PasswordHash.SequenceEqual(dto.OldPassword.HashPassword(user.Salt)))
                    return BadRequest("Incorrect old password");

                user.PasswordHash = dto.NewPassword.HashPassword(user.Salt);
                await UpdateService.Update(user);

                await TokenManager.InvalidateAllTokensForUser(user.Id);
                return SetupTokens(user);
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage(false));
            }
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity)
                return Unauthorized();

            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim is null)
                return Unauthorized();

            try
            {
                await DeleteService.Delete(userIdClaim);
                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage(false));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public IActionResult AdminTest()
        {
            return Ok();
        }

        public class RegisterDTO
        {
            public string Name { get; set; } = null!;
            public string Email { get; set; } = null!;
            public string Password { get; set; } = null!;
        }

        public class LoginDTO
        {
            public string Email { get; set; } = null!;
            public string Password { get; set; } = null!;
        }

        public class ResetPasswordDTO
        {
            public string OldPassword { get; set; } = null!;
            public string NewPassword { get; set; } = null!;
        }

        [GeneratedRegex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$")]
        private static partial Regex ValidEmailRegex();
    }
}
