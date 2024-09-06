using FitnessTracker.DTOs.Requests.User;
using FitnessTracker.Models;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [Authorize]
        [HttpPost("pins/workout")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateWorkoutPin([FromBody] CreatePinsRequestDTO request)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            try
            {
                int currentPinCount = await readSingleSelectedService.Get(x => x.WorkoutPins.Count + x.SplitPins.Count, x => x.Id == userId);
                if (currentPinCount + request.NewPinIds.Count() > 6)
                    return BadRequest("Cannot have more than 6 pins");

                await workoutPinCreateRangeService.Add(request.NewPinIds.Select((x, i) => new WorkoutPin()
                {
                    UserId = userId,
                    WorkoutId = x,
                    Order = currentPinCount + i + 1
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetErrorMessage());
            }

            return Created();
        }

        [Authorize]
        [HttpPost("pins/split")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateSplitPin([FromBody] CreatePinsRequestDTO request)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            try
            {
                int currentPinCount = await readSingleSelectedService.Get(x => x.WorkoutPins.Count + x.SplitPins.Count, x => x.Id == userId);
                if (currentPinCount + request.NewPinIds.Count() > 6)
                    return BadRequest("Cannot have more than 6 pins");

                await splitPinCreateRangeService.Add(request.NewPinIds.Select((x, i) => new SplitPin()
                {
                    UserId = userId,
                    SplitId = x,
                    Order = currentPinCount + i + 1
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetErrorMessage());
            }

            return Created();
        }
    }
}
