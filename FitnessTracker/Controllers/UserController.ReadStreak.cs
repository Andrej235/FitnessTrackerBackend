using FitnessTracker.DTOs.Responses.CompletedWorkouts;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [HttpGet("{username}/streak")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWeekOfCompletedWorkoutsResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserStreak(string username, [FromQuery] int? year) => Ok(await userService.GetStreak(username, year));

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("{username}/streak/week/{date:datetime}")]
        [ProducesResponseType(typeof(DetailedWeekOfCompletedWorkoutsResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserStreakOnWeek(string username, DateTime date) => Ok(await userService.GetUserStreakOnWeek(username, date));
    }
}
