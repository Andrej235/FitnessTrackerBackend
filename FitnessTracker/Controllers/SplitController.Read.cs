using FitnessTracker.DTOs.Responses.Split;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class SplitController
    {
        [HttpGet("simple/by/{username}")]
        [ProducesResponseType(typeof(IEnumerable<SimpleSplitResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBy(string username, [FromQuery] string? nameFilter, [FromQuery] int? offset, [FromQuery] int? limit) => Ok(await splitService.GetAllPublicBy(username, nameFilter, offset, limit));

        [Authorize]
        [HttpGet("personal/simple")]
        [ProducesResponseType(typeof(IEnumerable<SimpleSplitResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllPersonal([FromQuery] string? nameFilter, [FromQuery] int? offset, [FromQuery] int? limit)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            return Ok(await splitService.GetAllPersonal(userId, nameFilter, offset, limit));
        }

        [Authorize]
        [HttpGet("favorite/simple")]
        [ProducesResponseType(typeof(IEnumerable<SimpleSplitResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllFavorites([FromQuery] string? nameFilter, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            return Ok(await splitService.GetAllFavorites(userId, nameFilter, limit, offset));
        }

        [Authorize]
        [HttpGet("liked/simple")]
        [ProducesResponseType(typeof(IEnumerable<SimpleSplitResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllLiked([FromQuery] string? nameFilter, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            return Ok(await splitService.GetAllLiked(userId, nameFilter, limit, offset));
        }

        [Authorize]
        [HttpGet("favorite/simple/by/{username}")]
        [ProducesResponseType(typeof(IEnumerable<SimpleSplitResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllFavoritesBy(string username, [FromQuery] string? nameFilter, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Ok(await splitService.GetAllFavoritesBy(username, null, nameFilter, limit, offset));
            else
                return Ok(await splitService.GetAllFavoritesBy(username, userId, nameFilter, limit, offset));
        }

        [Authorize]
        [HttpGet("liked/simple/by/{username}")]
        [ProducesResponseType(typeof(IEnumerable<SimpleSplitResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllLikedBy(string username, [FromQuery] string? nameFilter, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Ok(await splitService.GetAllLikedBy(username, null, nameFilter, limit, offset));
            else
                return Ok(await splitService.GetAllLikedBy(username, userId, nameFilter, limit, offset));
        }

        [Authorize]
        [HttpGet("{id:guid}/detailed")]
        [ProducesResponseType(typeof(DetailedSplitResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetailed(Guid id)
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
