using FitnessTracker.DTOs.Responses.Workout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class SplitController
    {
        [Authorize]
        [HttpGet("me/workoutoptions")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWorkoutOptionResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetSplitWorkoutOptions([FromQuery] int? limit, [FromQuery] int? offset, [FromQuery] string? searchTerm, [FromQuery] bool? publicOnly, [FromQuery] bool? favoriteOnly, [FromQuery] bool? personalOnly)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            if (searchTerm is null && publicOnly is null && favoriteOnly is null)
                personalOnly = true;

            return Ok(await splitService.GetSplitWorkoutOptions(userId, limit, offset, searchTerm, publicOnly, favoriteOnly, personalOnly));
        }
    }
}
