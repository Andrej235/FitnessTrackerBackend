using FitnessTracker.DTOs.Requests.Pins;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [Authorize]
        [HttpDelete("pins")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteWorkoutPin([FromBody] DeletePinsRequestDTO request)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            try
            {
                IEnumerable<DeleteSinglePinRequestDTO> deletedWorkouts = request.DeletedPins.Where(x => x.Type == DTOs.Enums.PinType.Workout);
                IEnumerable<DeleteSinglePinRequestDTO> deletedSplits = request.DeletedPins.Where(x => x.Type == DTOs.Enums.PinType.Split);

                foreach (DeleteSinglePinRequestDTO? workout in deletedWorkouts)
                    await workoutPinDeleteService.Delete(x => x.UserId == userId && x.WorkoutId == workout.Id); //TODO: Replace with a real deleterange

                foreach (DeleteSinglePinRequestDTO? split in deletedSplits)
                    await splitPinDeleteService.Delete(x => x.UserId == userId && x.SplitId == split.Id); //TODO: Replace with a real deleterange
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetErrorMessage());
            }

            return NoContent();
        }
    }
}
