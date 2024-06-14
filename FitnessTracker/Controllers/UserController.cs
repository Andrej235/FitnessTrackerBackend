using FitnessTracker.Auth;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FitnessTracker.DTOs;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Mapping;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Update;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace FitnessTracker.Controllers
{
    [Route("api/user")]
    [ApiController]
    public partial class UserController(IReadService<User> readService,
                          IEntityMapper<User, UserDTO> mapper,
                          ICreateService<User> createService,
                          IDeleteService<User> deleteService,
                          IUpdateService<User> updateService,
                          ITokenManager tokenManager) : ControllerBase
    {
        public IReadService<User> ReadService { get; } = readService;
        public IEntityMapper<User, UserDTO> Mapper { get; } = mapper;
        public ICreateService<User> CreateService { get; } = createService;
        public IDeleteService<User> DeleteService { get; } = deleteService;
        public IUpdateService<User> UpdateService { get; } = updateService;
        public ITokenManager TokenManager { get; } = tokenManager;

        private async Task<OkObjectResult> SetupTokens(User user)
        {
            var jwt = await TokenManager.GenerateJWTAndRefreshToken(user, Response.Cookies);
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
                PasswordHash = userDTO.Password.ToHash(salt),
                Role = Role.User
            };

            var newEntityId = await CreateService.Add(user);
            if (newEntityId == default)
                return BadRequest("User already exists");

            return await SetupTokens(user);
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
                if (user is null)
                    return BadRequest("Incorrect email or password");

                var hash = userDTO.Password.ToHash(user.Salt);
                if (!user.PasswordHash.SequenceEqual(hash))
                    return BadRequest("Incorrect email or password");

                return await SetupTokens(user);
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage(false));
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Authenticate()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity)
                return Unauthorized();

            var userIdClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim is null)
                return Unauthorized();

            var user = await ReadService.Get(userIdClaim, "all");
            if (user is null)
                return Unauthorized();

            return Ok(Mapper.Map(user));
        }

        [Authorize(AuthenticationSchemes = "AllowExpired")]
        [HttpGet("refresh")]
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
                    return Unauthorized("Invalid token");

                var newJwt = await TokenManager.RefreshJWT(jwtId, refreshToken, userId);
                return Ok(newJwt);
            }
            catch (Exception)
            {
                return BadRequest("Invalid token");
            }
        }

        [Authorize]
        [HttpDelete("logout")]
        public async Task<IActionResult> Logout()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshTokenString) || !Guid.TryParse(refreshTokenString, out var refreshToken))
                return Unauthorized("Invalid token");

            await TokenManager.InvalidateRefreshToken(refreshToken);
            return Ok();
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

            var user = await ReadService.Get(userIdClaim, "none");
            if (user is null)
                return Unauthorized();

            if (!user.PasswordHash.SequenceEqual(dto.OldPassword.ToHash(user.Salt)))
                return BadRequest("Incorrect old password");

            user.PasswordHash = dto.NewPassword.ToHash(user.Salt);
            await UpdateService.Update(user);

            await TokenManager.InvalidateAllTokensForUser(user.Id);
            return await SetupTokens(user);
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

        [Authorize(Roles = Role.Admin)]
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
