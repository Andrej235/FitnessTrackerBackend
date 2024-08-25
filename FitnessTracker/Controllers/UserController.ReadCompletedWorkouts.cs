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

            IEnumerable<IGrouping<DateTime, Models.CompletedWorkout>> groupedCompletedWorkouts = (await completedWorkoutReadRangeService.Get(x => x.UserId == userId, null, null, "overview"))
                .GroupBy(x => x.CompletedAt.GetStartOfWeek());

            if (year is null)
            {
                DateTime startOfWeek = DateTime.Now.GetStartOfWeek();
                DateTime startOfLastYearsWeek = startOfWeek.AddYears(-1).GetStartOfWeek().AddDays(7);
                groupedCompletedWorkouts = groupedCompletedWorkouts.Where(x => x.Key > startOfLastYearsWeek && x.Key <= startOfWeek);
            }
            else
                groupedCompletedWorkouts = groupedCompletedWorkouts.Where(x => x.Key.Year == year);

            IEnumerable<SimpleWeekOfCompletedWorkoutsResponseDTO> mapped = groupedCompletedWorkouts.Select(simpleWeekOfCompletedWorkoutsResponseMapper.Map);
            return Ok(mapped);
        }

        [HttpGet("{username:guid}/streak")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWeekOfCompletedWorkoutsResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUserStreak(string username, [FromQuery] int? year)
        {
            Models.User? user = await readSingleService.Get(x => x.Username == username, "detailed");
            if (user is null)
                return NotFound();

            IEnumerable<IGrouping<DateTime, Models.CompletedWorkout>> groupedCompletedWorkouts = (await completedWorkoutReadRangeService.Get(x => x.UserId == user.Id, null, null, "overview"))
                .GroupBy(x => x.CompletedAt.GetStartOfWeek());

            if (year is null)
            {
                DateTime startOfWeek = DateTime.Now.GetStartOfWeek();
                DateTime startOfLastYearsWeek = startOfWeek.AddYears(-1).GetStartOfWeek().AddDays(7);
                groupedCompletedWorkouts = groupedCompletedWorkouts.Where(x => x.Key > startOfLastYearsWeek && x.Key <= startOfWeek);
            }
            else
                groupedCompletedWorkouts = groupedCompletedWorkouts.Where(x => x.Key.Year == year);

            IEnumerable<SimpleWeekOfCompletedWorkoutsResponseDTO> mapped = groupedCompletedWorkouts.Select(simpleWeekOfCompletedWorkoutsResponseMapper.Map);
            return Ok(mapped);
        }

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

            DateTime startOfWeek = date.GetStartOfWeek();
            DateTime endOfWeek = startOfWeek.Date.AddDays(7);
            IEnumerable<Models.CompletedWorkout> completedWorkout = await completedWorkoutReadRangeService.Get(x => x.UserId == userId && x.CompletedAt >= startOfWeek && x.CompletedAt <= endOfWeek, null, null, "detailed");
            DetailedWeekOfCompletedWorkoutsResponseDTO mapped = detailedWeekOfCompletedWorkoutsResponseMapper.Map(completedWorkout);
            return Ok(mapped);
        }
    }
}
