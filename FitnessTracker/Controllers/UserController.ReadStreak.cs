using FitnessTracker.DTOs.Responses.CompletedWorkouts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [HttpGet("{username}/streak")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWeekOfCompletedWorkoutsResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserStreak(string username, [FromQuery] int? year)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Ok(await userService.GetStreak(username, null, year));
            else
                return Ok(await userService.GetStreak(username, userId, year));
        }

        [HttpGet("{username}/streak/week/{date:datetime}")]
        [ProducesResponseType(typeof(DetailedWeekOfCompletedWorkoutsResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserStreakOnWeek(string username, DateTime date)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Ok(await userService.GetUserStreakOnWeek(username, null, date));
            else
                return Ok(await userService.GetUserStreakOnWeek(username, userId, date));
        }
    }
}
