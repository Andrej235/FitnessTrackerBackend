using FitnessTracker.DTOs.Requests.Split;
using FitnessTracker.Models;
using FitnessTracker.Services.Read.Full;
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBasicInformation(Guid id, [FromBody] UpdateSplitBaseInfoRequestDTO request)
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out Guid userId))
                    return Unauthorized();

                Split? split = await readSingleService.Get(x => x.CreatorId == userId && x.Id == id, x => x.Include(x => x.Workouts).ThenInclude(x => x.Workout));
                if (split is null)
                    return NotFound();

                split.Name = request.Name;
                split.Description = request.Description;
                split.IsPublic = request.IsPublic && split.Workouts.All(x => x.Workout.IsPublic);

                await updateService.Update(split);
                return NoContent();

            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest();
            }
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPatch("{id:guid}/workout/{day:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSplitWorkout(Guid id, DayOfWeek day, [FromBody] UpdateSplitWorkoutRequestDTO request)
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out Guid userId))
                    return Unauthorized();

                Split? split = await readSingleService.Get(x => x.CreatorId == userId && x.Id == id, x => x.Include(x => x.Workouts).ThenInclude(x => x.Workout));
                SplitWorkout? splitWorkout = split?.Workouts.FirstOrDefault(x => x.Day == day);

                if (splitWorkout is null)
                    return NotFound();

                splitWorkout.WorkoutId = request.NewWorkoutId;
                await splitWorkoutUpdateService.Update(splitWorkout);
                return NoContent();

            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest();
            }
        }
    }
}
