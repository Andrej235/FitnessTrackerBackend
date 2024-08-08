using FitnessTracker.DTOs.Responses.Workout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [Authorize]
        [HttpGet("me/split/today")]
        [ProducesResponseType(typeof(DetailedWorkoutResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTodaysWorkout()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            var user = await readSingleService.Get(x => x.Id == userId, "todays workout");
            if (user is null)
                return Unauthorized();

            if (user.CurrentSplit is null)
                return NotFound();

            var workout = user.CurrentSplit.Workouts.FirstOrDefault();
            if (workout is null)
                return NotFound();

            var mapped = detailedWorkoutResponseMapper.Map(workout.Workout);
            return Ok(mapped);
        }
    }
}
