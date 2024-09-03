using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [Authorize]
        [HttpPost("pin/workout/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateWorkoutPins(Guid id)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            try
            {
                _ = await workoutPinCreateService.Add(new()
                {
                    UserId = userId,
                    WorkoutId = id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetErrorMessage());
            }

            return Created();
        }

        [Authorize]
        [HttpPost("pin/split/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateSplitPins(Guid id)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            try
            {
                _ = await splitPinCreateService.Add(new()
                {
                    UserId = userId,
                    SplitId = id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetErrorMessage());
            }

            return Created();
        }
    }
}
