using FitnessTracker.DTOs.Requests.Split;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class SplitController
    {
        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateSplitRequestDTO request)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var selectedWorkoutIds = request.Workouts.Select(x => x.WorkoutId);
            var selectedWorkouts = await workoutReadRangeService.Get(x => selectedWorkoutIds.Contains(x.Id), 0, 10, "none");
            if (selectedWorkoutIds.Count() != selectedWorkouts.Count())
                return NotFound("One or more selected workouts could not found");

            if (selectedWorkouts.Any(x => !x.IsPublic && x.CreatorId != userId))
                return BadRequest("One or more selected workouts are not public and you are not the creator");

            if (request.IsPublic && selectedWorkouts.Any(x => !x.IsPublic))
                return BadRequest("Attempted to create a public split with one or more private workouts");

            var mapped = createRequestMapper.Map(request);
            mapped.CreatorId = userId;

            await createService.Add(mapped);
            return Created();
        }
    }
}
