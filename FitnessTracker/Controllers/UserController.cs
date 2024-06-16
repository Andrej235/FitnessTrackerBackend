using FitnessTracker.Auth;
using FitnessTracker.DTOs.Requests.User;
using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Emails;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Mapping.Request;
using FitnessTracker.Services.Mapping.Response;
using FitnessTracker.Services.Read;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace FitnessTracker.Controllers
{
    [Route("api/user")]
    [ApiController]
    public partial class UserController(IRequestMapper<RegisterUserRequestDTO, User> registrationMapper,
                                        IResponseMapper<User, DetailedUserResponseDTO> detailedResponseMapper,
                                        ICreateService<User> createService,
                                        IReadService<User> readService,
                                        ITokenManager tokenManager,
                                        IEmailConformationService emailConformationService) : ControllerBase
    {
        private readonly IRequestMapper<RegisterUserRequestDTO, User> registrationMapper = registrationMapper;
        private readonly ICreateService<User> createService = createService;
        private readonly IReadService<User> readService = readService;
        private readonly ITokenManager tokenManager = tokenManager;
        private readonly IEmailConformationService emailConformationService = emailConformationService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDTO request)
        {
            try
            {
                if (request.Name.Length < 3 || !ValidEmailRegex().IsMatch(request.Email.Trim()) || request.Password.Length < 8)
                    return BadRequest("Invalid registration details");

                var user = registrationMapper.Map(request);
                var newUserId = await createService.Add(user);
                if (newUserId == default)
                    return BadRequest("User already exists");

                var jwt = await tokenManager.GenerateJWTAndRefreshToken(user, Response.Cookies);
                await emailConformationService.SendEmailConfirmation(user.Email, user.Id);

                return Ok(jwt);
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage());
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestDTO request)
        {
            if (!ValidEmailRegex().IsMatch(request.Email) || request.Password.Length < 8)
                return BadRequest("Incorrect email or password");

            var user = await readService.Get(x => x.Email == request.Email, "none");
            if (user is null)
                return BadRequest("Incorrect email or password");

            var hash = request.Password.ToHash(user.Salt);
            if (!user.PasswordHash.SequenceEqual(hash))
                return BadRequest("Incorrect email or password");

            var jwt = await tokenManager.GenerateJWTAndRefreshToken(user, Response.Cookies);
            return Ok(jwt);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetDetailed()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var user = await readService.Get(x => x.Id == userId, "all");
            if (user is null)
                return Unauthorized();

            return Ok(detailedResponseMapper.Map(user));
        }

        [Authorize(AuthenticationSchemes = "AllowExpired")]
        [HttpGet("refresh")]
        public async Task<IActionResult> Refresh()
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || claimsIdentity.FindFirst(JwtRegisteredClaimNames.Jti)?.Value is not string jwtIdString
                    || !Request.Cookies.TryGetValue("refreshToken", out var refreshTokenString)
                    || !Guid.TryParse(jwtIdString, out var jwtId)
                    || !Guid.TryParse(userIdString, out var userId)
                    || !Guid.TryParse(refreshTokenString, out var refreshToken))
                    return Unauthorized("Invalid token");

                var newJwt = await tokenManager.RefreshJWT(jwtId, refreshToken, userId);
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

            await tokenManager.InvalidateRefreshToken(refreshToken);
            return Ok();
        }

        [Authorize(Roles = Role.Unverified)]
        [HttpPost("confirm/{code:guid}")]
        public async Task<IActionResult> Confirm(Guid code)
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out var userId))
                    return Unauthorized();

                var success = await emailConformationService.ConfirmEmail(userId, code);
                return success ? Ok("Email Confirmed") : BadRequest("Invalid code");
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage());
            }
        }

        [Authorize(Roles = Role.Unverified)]
        [HttpPost("resendconformationemail")]
        public async Task<IActionResult> GetMail()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var user = await readService.Get(x => x.Id == userId, "none");
            if (user is null)
                return Unauthorized();

            await emailConformationService.SendEmailConfirmation(user.Email, userId);
            return Ok();
        }

        [GeneratedRegex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$")]
        private static partial Regex ValidEmailRegex();
    }
}
