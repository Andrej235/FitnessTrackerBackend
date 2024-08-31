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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] CreateSplitRequestDTO request)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            IEnumerable<Guid> selectedWorkoutIds = request.Workouts.Select(x => x.WorkoutId);
            IEnumerable<Models.Workout> selectedWorkouts = await workoutReadRangeService.Get(x => selectedWorkoutIds.Contains(x.Id));
            if (selectedWorkoutIds.Count() != selectedWorkouts.Count())
                return NotFound("One or more selected workouts could not found");

            if (selectedWorkouts.Any(x => !x.IsPublic && x.CreatorId != userId))
                return BadRequest("One or more selected workouts are not public and you are not the creator");

            if (request.IsPublic && selectedWorkouts.Any(x => !x.IsPublic))
                return BadRequest("Attempted to create a public split with one or more private workouts");

            Models.Split mapped = createRequestMapper.Map(request);
            mapped.CreatorId = userId;

            _ = await createService.Add(mapped);
            return Created();
        }
    }
}
