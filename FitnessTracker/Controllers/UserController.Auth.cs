using FitnessTracker.DTOs.Requests.User;
using FitnessTracker.DTOs.Responses.AuthTokens;
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
        [ProducesResponseType(typeof(SimpleJWTResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDTO request)
        {
            try
            {
                if (request.Username.Length < 3 || !ValidEmailRegex().IsMatch(request.Email.Trim()) || request.Password.Length < 8)
                    return BadRequest("Invalid registration details");

                Models.User mapped = registrationMapper.Map(request);
                Models.User newUser = await createService.Add(mapped);

                string jwt = await tokenManager.GenerateJWTAndRefreshToken(newUser, Response.Cookies);
                await emailConfirmationSender.SendEmailConfirmation(newUser.Email, newUser.Id);

                return Created($"/user/{newUser.Id}", jwtResponseMapper.Map(jwt));
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest(ex.GetErrorMessage());
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(SimpleJWTResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestDTO request)
        {
            if (!ValidEmailRegex().IsMatch(request.Email) || request.Password.Length < 8)
                return BadRequest("Incorrect email or password");

            Models.User? user = await readSingleService.Get(x => x.Email == request.Email);
            if (user is null)
                return BadRequest("Incorrect email or password");

            byte[] hash = request.Password.ToHash(user.Salt);
            if (!user.PasswordHash.SequenceEqual(hash))
                return BadRequest("Incorrect email or password");

            string jwt = await tokenManager.GenerateJWTAndRefreshToken(user, Response.Cookies);
            return Created("/api/user/me", jwtResponseMapper.Map(jwt));
        }

        [Authorize(AuthenticationSchemes = "AllowExpired")]
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(SimpleJWTResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh()
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || claimsIdentity.FindFirst(JwtRegisteredClaimNames.Jti)?.Value is not string jwtIdString
                    || !Request.Cookies.TryGetValue("refreshToken", out string? refreshTokenString)
                    || !Guid.TryParse(jwtIdString, out Guid jwtId)
                    || !Guid.TryParse(userIdString, out Guid userId)
                    || !Guid.TryParse(refreshTokenString, out Guid refreshToken))
                    return Unauthorized("Invalid token");

                string newJwt = await tokenManager.RefreshJWT(jwtId, refreshToken, userId);
                return Created("/api/user/me", jwtResponseMapper.Map(newJwt));
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest("Invalid token");
            }
        }

        [Authorize]
        [HttpDelete("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Logout()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out string? refreshTokenString) || !Guid.TryParse(refreshTokenString, out Guid refreshToken))
                return Unauthorized("Invalid token");

            await tokenManager.InvalidateRefreshToken(refreshToken);
            return NoContent();
        }
    }
}
