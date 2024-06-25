using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("me/completedworkouts")]
        public async Task<IActionResult> GetCompletedWorkouts()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var completedWorkouts = (await completedWorkoutReadRangeService.Get(x => x.UserId == userId, null, null, "detailed"))
                .GroupBy(x => GetStartOfWeek(x.CompletedAt))
                .Select(x => new
                {
                    Date = x.Key,
                    Count = x.Count(),
                    Split = x.First().Split.Workouts.Count
                });

            return Ok(completedWorkouts);
        }

        public static DateTime GetStartOfWeek(DateTime date)
        {
            int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-diff).Date;
        }
    }
}
