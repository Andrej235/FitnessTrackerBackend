using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class WorkoutController
    {
        [HttpGet("public/simple")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWorkoutResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllSimplePublic([FromQuery] string? name, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            var workouts = name is null
                ? await readRangeService.Get(x => x.IsPublic, offset, limit ?? 10, "creator")
                : await readRangeService.Get(x => x.IsPublic && EF.Functions.Like(x.Name, $"%{name}%"), offset, limit ?? 10, "creator");

            return Ok(workouts.Select(simpleResponseMapper.Map));
        }

        [HttpGet("public/simple/by/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWorkoutResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllSimplePublic(Guid userId, [FromQuery] string? name, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            var workouts = name is null
                ? await readRangeService.Get(x => x.CreatorId == userId && x.IsPublic, offset, limit ?? 10, "creator")
                : await readRangeService.Get(x => x.CreatorId == userId && x.IsPublic && EF.Functions.Like(x.Name, $"%{name}%"), offset, limit ?? 10, "creator");

            return Ok(workouts.Select(simpleResponseMapper.Map));
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("personal/simple")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWorkoutResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllSimplePersonal([FromQuery] string? name, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var workouts = name is null
                ? await readRangeService.Get(x => x.CreatorId == userId, offset, limit ?? 10, "creator")
                : await readRangeService.Get(x => x.CreatorId == userId && EF.Functions.Like(x.Name, $"%{name}%"), offset, limit ?? 10, "creator");

            return Ok(workouts.Select(simpleResponseMapper.Map));
        }

        [Authorize]
        [HttpGet("favorite/simple")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWorkoutResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllSimpleFavorites([FromQuery] string? name, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var workouts = name is null
                ? await readRangeService.Get(x => (x.IsPublic || x.CreatorId == userId) && x.Favorites.Any(x => x.Id == userId), offset, limit ?? 10, "creator,favorites")
                : await readRangeService.Get(x => (x.IsPublic || x.CreatorId == userId) && x.Favorites.Any(x => x.Id == userId) && EF.Functions.Like(x.Name, $"%{name}%"), offset, limit ?? 10, "creator,favorites");

            return Ok(workouts.Select(simpleResponseMapper.Map));
        }

        [Authorize]
        [HttpGet("{id:guid}/detailed")]
        [ProducesResponseType(typeof(DetailedWorkoutResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetailed(Guid id)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var workout = await readSingleService.Get(x => x.Id == id, "detailed");
            if (workout is null)
                return NotFound();

            if (!workout.IsPublic && workout.CreatorId != userId)
                return Unauthorized();

            var mapped = detailedResponseMapper.Map(workout);
            mapped.IsLiked = workout.Likes.Any(x => x.Id == userId);
            mapped.IsFavorited = workout.Favorites.Any(x => x.Id == userId);
            return Ok(mapped);
        }
    }
}
