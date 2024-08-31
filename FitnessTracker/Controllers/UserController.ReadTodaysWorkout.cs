using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Services.Read;
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

            DayOfWeek today = DateTime.Today.DayOfWeek;
            Models.User? user = await readSingleService.Get(
                x => x.Id == userId,
                x => x.Include(x => x.CurrentSplit!)
                      .ThenInclude(x => x.Workouts.Where(w => w.Day == today))
                      .ThenInclude(x => x.Workout)
                      .ThenInclude(x => x.Sets)
                      .ThenInclude(x => x.Exercise));

            if (user is null)
                return Unauthorized();

            if (user.CurrentSplit is null)
                return NotFound();

            Models.SplitWorkout? workout = user.CurrentSplit.Workouts.FirstOrDefault();
            if (workout is null)
                return NotFound();

            DetailedWorkoutResponseDTO mapped = detailedWorkoutResponseMapper.Map(workout.Workout);
            return Ok(mapped);
        }
    }
}
