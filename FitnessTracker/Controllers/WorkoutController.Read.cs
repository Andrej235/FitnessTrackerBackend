using FitnessTracker.DTOs.Responses.Workout;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class WorkoutController
    {
        [HttpGet("simple/by/{username}")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWorkoutResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllBy(string username, [FromQuery] string? nameFilter, [FromQuery] int? limit, [FromQuery] int? offset) => Ok(await workoutService.GetAllBy(username, nameFilter, limit, offset));

        [HttpGet("favorite/simple/by/{username}")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWorkoutResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllFavoritesBy(string username, [FromQuery] string? nameFilter, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Ok(await workoutService.GetAllFavoritesBy(username, null, nameFilter, limit, offset));
            else
                return Ok(await workoutService.GetAllFavoritesBy(username, userId, nameFilter, limit, offset));
        }

        [HttpGet("liked/simple/by/{username}")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWorkoutResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllLikedBy(string username, [FromQuery] string? nameFilter, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Ok(await workoutService.GetAllLikedBy(username, null, nameFilter, limit, offset));
            else
                return Ok(await workoutService.GetAllLikedBy(username, userId, nameFilter, limit, offset));
        }

        [HttpGet("{creator}/{name}")]
        [ProducesResponseType(typeof(DetailedWorkoutResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetailed(string creator, string name)
        {
            Guid userId = default;
            if (User.Identity is ClaimsIdentity claimsIdentity
                && claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is string userIdString)
                _ = Guid.TryParse(userIdString, out userId);

            return Ok(await workoutService.GetDetailed(creator, name, userId));
        }
    }
}
