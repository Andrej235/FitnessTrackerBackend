using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

            var completedWorkouts = (await completedWorkoutReadRangeService.Get(x => x.UserId == userId, null, null, "overview"))
                .GroupBy(x => x.CompletedAt.GetStartOfWeek())
                .Select(simpleWeekOfCompletedWorkoutsResponseMapper.Map);

            return Ok(completedWorkouts);
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("me/completedworkouts/week/{date:datetime}")]
        public async Task<IActionResult> GetCompletedWorkouts(DateTime date)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var startOfWeek = date.GetStartOfWeek();
            var endOfWeek = startOfWeek.Date.AddDays(7);
            var completedWorkout = await completedWorkoutReadRangeService.Get(x => x.UserId == userId && x.CompletedAt >= startOfWeek && x.CompletedAt <= endOfWeek, null, null, "detailed");
            var mapped = detailedWeekOfCompletedWorkoutsResponseMapper.Map(completedWorkout);
            return Ok(mapped);
        }
    }
}
