using FitnessTracker.DTOs.Responses.CompletedWorkouts;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [Authorize]
        [HttpGet("me/streak")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWeekOfCompletedWorkoutsResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUserStreak([FromQuery] int? year)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            return Ok(await userService.GetStreak(userId, year));
        }

        [HttpGet("{username}/streak")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWeekOfCompletedWorkoutsResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserStreak(string username, [FromQuery] int? year) => Ok(await userService.GetStreak(username, year));

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("me/streak/week/{date:datetime}")]
        [ProducesResponseType(typeof(DetailedWeekOfCompletedWorkoutsResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUserStreakOnWeek(DateTime date)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            return Ok(await userService.GetUserStreakOnWeek(userId, date));
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("{username}/streak/week/{date:datetime}")]
        [ProducesResponseType(typeof(DetailedWeekOfCompletedWorkoutsResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserStreakOnWeek(string username, DateTime date) => Ok(await userService.GetUserStreakOnWeek(username, date));
    }
}
