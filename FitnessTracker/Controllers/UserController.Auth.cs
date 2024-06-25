using FitnessTracker.DTOs.Requests.User;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
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
                await emailConfirmationSender.SendEmailConfirmation(user.Email, user.Id);

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

            var user = await readSingleService.Get(x => x.Email == request.Email, "none");
            if (user is null)
                return BadRequest("Incorrect email or password");

            var hash = request.Password.ToHash(user.Salt);
            if (!user.PasswordHash.SequenceEqual(hash))
                return BadRequest("Incorrect email or password");

            var jwt = await tokenManager.GenerateJWTAndRefreshToken(user, Response.Cookies);
            return Ok(jwt);
        }

        [Authorize(AuthenticationSchemes = "AllowExpired")]
        [HttpPost("refresh")]
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
    }
}
