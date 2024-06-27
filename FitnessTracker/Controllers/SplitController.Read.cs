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
        public async Task<IActionResult> GetAllSimplePublic()
        {
            var workouts = await readRangeService.Get(x => x.IsPublic, 0, 10, "creator");
            return Ok(workouts.Select(simpleResponseMapper.Map));
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("personal/simple")]
        [ProducesResponseType(typeof(IEnumerable<SimpleSplitResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllSimplePersonal()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var workouts = await readRangeService.Get(x => x.CreatorId == userId, 0, 10, "creator");
            return Ok(workouts.Select(simpleResponseMapper.Map));
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
