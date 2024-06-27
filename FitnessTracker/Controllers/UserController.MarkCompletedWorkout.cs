using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPost("me/split/today/markascomplete")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> MarkCompletedWorkout()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var user = await readSingleService.Get(x => x.Id == userId, "split,splitworkouts");
            if (user is null)
                return Unauthorized();

            if (user.CurrentSplit is null)
                return BadRequest("No current split");

            await completedWorkoutcreateService.Add(new()
            {
                UserId = userId,
                SplitId = user.CurrentSplit.Id,
                CompletedAt = DateTime.UtcNow,
                WorkoutId = user.CurrentSplit.Workouts.First().WorkoutId,
            });

            return Created();
        }

        /*        [Authorize(Roles = Role.Admin)]
                [HttpPost("unsafe/completedworkout")]
                public async Task<IActionResult> Test()
                {
                    if (User.Identity is not ClaimsIdentity claimsIdentity
                        || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                        || !Guid.TryParse(userIdString, out var userId))
                        return Unauthorized();

                    var today = DateTime.UtcNow;
                    var current = today.AddMonths(-18).GetStartOfWeek();
                    int weekCount = 0;
                    Guid workoutId = Guid.Parse("852da05b-0937-4775-61fe-08dc943d34c0");
                    Guid splitId = Guid.Parse("aabe499b-f1a7-496e-2606-08dc9523ddbd");

                    while (current < today)
                    {
                        current = current.AddDays(1);
                        weekCount++;
                        weekCount %= 7;

                        if (weekCount > 4 || Random.Shared.NextDouble() < 0.5)
                            continue;

                        await completedWorkoutcreateService.Add(new()
                        {
                            UserId = userId,
                            WorkoutId = workoutId,
                            SplitId = splitId,
                            CompletedAt = current,
                        });
                    }

                    return Ok();
                }*/
    }
}
