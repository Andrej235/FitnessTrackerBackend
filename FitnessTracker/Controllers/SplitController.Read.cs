using FitnessTracker.DTOs.Responses.Split;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class SplitController
    {
        [HttpGet("simple/by/{username}")]
        [ProducesResponseType(typeof(IEnumerable<SimpleSplitResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllBy(string username, [FromQuery] string? nameFilter, [FromQuery] int? offset, [FromQuery] int? limit) => Ok(await splitService.GetAllPublicBy(username, nameFilter, offset, limit));

        [HttpGet("favorite/simple/by/{username}")]
        [ProducesResponseType(typeof(IEnumerable<SimpleSplitResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllFavoritesBy(string username, [FromQuery] string? nameFilter, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Ok(await splitService.GetAllFavoritesBy(username, null, nameFilter, limit, offset));
            else
                return Ok(await splitService.GetAllFavoritesBy(username, userId, nameFilter, limit, offset));
        }

        [HttpGet("liked/simple/by/{username}")]
        [ProducesResponseType(typeof(IEnumerable<SimpleSplitResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllLikedBy(string username, [FromQuery] string? nameFilter, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Ok(await splitService.GetAllLikedBy(username, null, nameFilter, limit, offset));
            else
                return Ok(await splitService.GetAllLikedBy(username, userId, nameFilter, limit, offset));
        }

        [HttpGet("{creator}/{name}")]
        [ProducesResponseType(typeof(DetailedSplitResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetailed(string creator, string name)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Ok(await splitService.GetSingleDetailed(creator, name, null));
            else
                return Ok(await splitService.GetSingleDetailed(creator, name, userId));
        }

        [HttpGet("usedby/{username}/detailed")]
        [ProducesResponseType(typeof(DetailedUserSplitResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetailedUsedBy(string username) => Ok(await splitService.GetDetailedUsedBy(username));
    }
}
