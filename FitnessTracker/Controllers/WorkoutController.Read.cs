using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class WorkoutController
    {
        [HttpGet("public/simple")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWorkoutResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] string? name, [FromQuery] int? limit, [FromQuery] int? offset) => Ok(await workoutService.GetAll(name, limit, offset));

        [HttpGet("public/simple/by/{username}")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWorkoutResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBy(string username, [FromQuery] string? name, [FromQuery] int? limit, [FromQuery] int? offset) => Ok(await workoutService.GetAllBy(username, name, limit, offset));

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("personal/simple")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWorkoutResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllPersonal([FromQuery] string? name, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            return Ok(await workoutService.GetAllPersonal(userId, name, limit, offset));
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("favorite/simple")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWorkoutResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllFavorites([FromQuery] string? name, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            return Ok(await workoutService.GetAllFavorites(userId, name, limit, offset));
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("liked/simple")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWorkoutResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllLiked([FromQuery] string? name, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            return Ok(await workoutService.GetAllLiked(userId, name, limit, offset));
        }

        [HttpGet("{id:guid}/detailed")]
        [ProducesResponseType(typeof(DetailedWorkoutResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetailed(Guid id)
        {
            Guid userId = default;
            if (User.Identity is ClaimsIdentity claimsIdentity
                && claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is string userIdString)
                _ = Guid.TryParse(userIdString, out userId);

            return Ok(await workoutService.GetDetailed(id, userId));
        }
    }
}
