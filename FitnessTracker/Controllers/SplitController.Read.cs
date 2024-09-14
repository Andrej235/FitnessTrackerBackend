using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class SplitController
    {
        [HttpGet("public/simple")]
        [ProducesResponseType(typeof(IEnumerable<SimpleSplitResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPublic([FromQuery] string? name) => Ok(await splitService.GetAllPublic(name));

        [HttpGet("public/simple/by/{username}")]
        [ProducesResponseType(typeof(IEnumerable<SimpleSplitResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPublicBy(string username, [FromQuery] string? name) => Ok(await splitService.GetAllPublicBy(username, name));

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("me/simple")]
        [ProducesResponseType(typeof(IEnumerable<SimpleSplitResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllSimplePersonal([FromQuery] string? name)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            return Ok(await splitService.GetAllSimplePersonal(userId, name));
        }

        [Authorize]
        [HttpGet("{id:guid}/detailed")]
        [ProducesResponseType(typeof(DetailedSplitResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSingleDetailed(Guid id)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            return Ok(await splitService.GetSingleDetailed(id, userId));
        }

        [HttpGet("usedby/{username}/detailed")]
        [ProducesResponseType(typeof(DetailedUserSplitResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetailedUsedBy(string username) => Ok(await splitService.GetDetailedUsedBy(username));
    }
}
