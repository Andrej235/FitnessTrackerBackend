using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class WorkoutController
    {
        [HttpGet("{creator}/{name}/exercise-chart-data/{exerciseId:int}")]
        public async Task<IActionResult> GetChartData(string creator, string name, int exerciseId, [FromQuery] int? offset, [FromQuery] int? limit)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            return Ok(await workoutService.GetChartDataForExercise(userId, creator, name, exerciseId, offset, limit));
        }
    }
}
