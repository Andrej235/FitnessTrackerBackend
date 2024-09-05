using FitnessTracker.DTOs.Requests.User;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [Authorize]
        [HttpDelete("pins/workout")]
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
                foreach (Guid id in request.DeletedPinIds)
                    await workoutPinDeleteService.Delete(x => x.UserId == userId && x.WorkoutId == id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetErrorMessage());
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("pins/split")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteSplitPin([FromBody] DeletePinsRequestDTO request)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            try
            {
                foreach (Guid id in request.DeletedPinIds)
                    await splitPinDeleteService.Delete(x => x.UserId == userId && x.SplitId == id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetErrorMessage());
            }

            return NoContent();
        }
    }
}
