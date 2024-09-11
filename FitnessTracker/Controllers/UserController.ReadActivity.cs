using FitnessTracker.DTOs.Responses.Activity;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [HttpGet]
        [Route("me/activity/latest")]
        [ProducesResponseType(typeof(UserActivityResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ReadLatestActivity()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();


            DateTime startOfWeek = DateTime.Now.GetStartOfWeek().AddDays(7);
            DateTime startOfLastYearsWeek = startOfWeek.AddYears(-1).GetStartOfWeek().AddDays(7);
            UserActivityResponseDTO? activity = await readSingleSelectedService.Get(
                    select: x => new UserActivityResponseDTO
                    {
                        UniqueSplitsCount = x.CompletedWorkouts
                            .Where(x => x.CompletedAt > startOfLastYearsWeek && x.CompletedAt <= startOfWeek)
                            .GroupBy(x => x.SplitId)
                            .Count(),
                        UniqueWorkoutsCount = x.CompletedWorkouts
                            .Where(x => x.CompletedAt > startOfLastYearsWeek && x.CompletedAt <= startOfWeek)
                            .GroupBy(x => x.WorkoutId != null ? x.WorkoutId : default)
                            .Count(),
                        MostCompletedWorkout = x.CompletedWorkouts
                            .Where(x => x.CompletedAt > startOfLastYearsWeek && x.CompletedAt <= startOfWeek)
                            .GroupBy(x => x.WorkoutId)
                            .Select(x => new UserActivityWorkoutStatsResponseDTO
                            {
                                Workout = x.Select(x => new ActivityWorkoutPreviewResponseDTO
                                {
                                    Id = x.WorkoutId,
                                    Name = x.Workout != null ? x.Workout.Name : null,
                                }).First(),
                                CompletionCount = x.Count(),
                            })
                            .OrderByDescending(x => x.CompletionCount)
                            .First(),
                    },
                    criteria: x => x.Id == userId);

            return Ok(activity);
        }
    }
}
