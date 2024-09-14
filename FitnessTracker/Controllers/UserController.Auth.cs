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
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDTO request) => Created((string?)null, await userService.Register(request, Response.Cookies));

        [HttpPost("login")]
        [ProducesResponseType(typeof(SimpleJWTResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestDTO request) => Created((string?)null, await userService.Login(request, Response.Cookies));

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

                return Created((string?)null, await userService.Refresh(jwtId, userId, refreshToken));
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

            await userService.Logout(refreshToken);
            return NoContent();
        }
    }
}
