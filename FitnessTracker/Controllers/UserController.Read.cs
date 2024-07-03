using FitnessTracker.DTOs.Responses.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [Authorize]
        [HttpGet("me/detailed")]
        [ProducesResponseType(typeof(DetailedUserResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetDetailed()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var user = await readSingleService.Get(x => x.Id == userId, "detailed");
            if (user is null)
                return Unauthorized();

            return Ok(detailedResponseMapper.Map(user));
        }

        [HttpGet("{id}/detailed")]
        [ProducesResponseType(typeof(DetailedUserResponseDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailed(Guid id)
        {
            var user = await readSingleService.Get(x => x.Id == id, "detailed");
            if (user is null)
                return Unauthorized();

            var mapped = publicUserDetailedResponseMapper.Map(user);

            if (User.Identity is ClaimsIdentity claimsIdentity
                && claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is string userIdString
                && Guid.TryParse(userIdString, out var userId))
                mapped.IsFollowing = user.Followers.Any(x => x.Id == userId);

            return Ok(mapped);
        }
    }
}
