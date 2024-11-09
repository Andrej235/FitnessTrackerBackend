using FitnessTracker.DTOs.Responses.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [Authorize]
        [HttpGet("basic")]
        [ProducesResponseType(typeof(SimpleUserResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetBasicInfo()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            return Ok(await userService.GetBasicInfo(userId));
        }

        [HttpGet("{username}/detailed")]
        [ProducesResponseType(typeof(DetailedUserResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetailed(string username)
        {
            Guid userId = default;
            if (User.Identity is ClaimsIdentity claimsIdentity
                && claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is string userIdString)
                _ = Guid.TryParse(userIdString, out userId);

            return Ok(await userService.GetDetailed(username, userId));
        }

        [HttpGet("{username}/followers")]
        [ProducesResponseType(typeof(IEnumerable<SimpleUserResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFollowersFor(string username, [FromQuery] string? searchTerm, [FromQuery] int? limit, [FromQuery] int? offset) => Ok(await userService.GetFollowersFor(username, searchTerm, offset, limit));

        [HttpGet("{username}/following")]
        [ProducesResponseType(typeof(IEnumerable<SimpleUserResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFollowingFor(string username, [FromQuery] string? searchTerm, [FromQuery] int? limit, [FromQuery] int? offset) => Ok(await userService.GetFollowingFor(username, searchTerm, offset, limit));
    }
}
