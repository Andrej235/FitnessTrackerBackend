using FitnessTracker.DTOs.Requests.Split;
using FitnessTracker.Models;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class SplitController
    {
        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPatch("{id:guid}/baseinfo")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSplitBaseInfoRequestDTO request)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var split = await readSingleService.Get(x => x.CreatorId == userId && x.Id == id, "workouts");
            if (split is null)
                return NotFound();

            split.Name = request.Name;
            split.Description = request.Description;
            split.IsPublic = request.IsPublic && split.Workouts.All(x => x.Workout.IsPublic);

            await updateService.Update(split);
            return NoContent();
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPatch("{id:guid}/workout/{day:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, DayOfWeek day, [FromBody] UpdateSplitWorkoutRequestDTO request)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var split = await readSingleService.Get(x => x.CreatorId == userId && x.Id == id, "workouts");
            var splitWorkout = split?.Workouts.FirstOrDefault(x => x.Day == day);

            if (splitWorkout is null)
                return NotFound();

            splitWorkout.WorkoutId = request.NewWorkoutId;
            await splitWorkoutUpdateService.Update(splitWorkout);
            return NoContent();
        }
    }
}
