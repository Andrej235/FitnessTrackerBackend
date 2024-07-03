using FitnessTracker.DTOs.Requests.Set;
using FitnessTracker.DTOs.Requests.Workout;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class WorkoutController
    {
        [HttpPatch("{id:guid}/baseinfo")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBasicInformation(Guid id, [FromBody] UpdateWorkoutBaseInfoRequestDTO request)
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out var userId))
                    return Unauthorized();

                var workout = await readSingleService.Get(x => x.CreatorId == userId && x.Id == id, "none");
                if (workout is null)
                    return NotFound();

                workout.Name = request.Name;
                workout.Description = request.Description;
                workout.IsPublic = request.IsPublic;

                await updateService.Update(workout);
                return NoContent();

            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest();
            }
        }

        [HttpPatch("{id:guid}/set/{setId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSet(Guid id, Guid setId, [FromBody] UpdateSetRequestDTO request)
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out var userId))
                    return Unauthorized();

                var workout = await readSingleService.Get(x => x.CreatorId == userId && x.Id == id, "sets");
                var set = workout?.Sets.FirstOrDefault(x => x.Id == setId);

                if (set is null)
                    return NotFound();

                set.ExerciseId = request.ExerciseId;
                set.TopRepRange = request.TopRepRange;
                set.BottomRepRange = request.BottomRepRange;
                set.NumberOfSets = request.NumberOfSets;

                await setUpdateService.Update(set);
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
