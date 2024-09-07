using FitnessTracker.DTOs.Enums;
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
        [HttpPost("pins")]
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
                if (!request.NewPins.Any())
                    return BadRequest("No pins provided");

                var pins = await readSingleSelectedService.Get(x => new
                {
                    Count = x.WorkoutPins.Count + x.SplitPins.Count,
                    HighestOrder = (x.WorkoutPins.Any() || x.SplitPins.Any())
                    ? x.WorkoutPins
                      .Select(wp => wp.Order)
                      .Union(x.SplitPins.Select(sp => sp.Order))
                      .Max()
                    : 0
                }, x => x.Id == userId);

                if (pins is null)
                    return NotFound();

                if (pins.Count + request.NewPins.Count() > 6)
                    return BadRequest("Cannot have more than 6 pins");

                IEnumerable<CreateSinglePinRequestDTO> workoutPinRequests = request.NewPins.Where(x => x.Type == PinType.Workout);
                IEnumerable<CreateSinglePinRequestDTO> splitPinRequests = request.NewPins.Where(x => x.Type == PinType.Split);
                int newWorkoutPinsCount = workoutPinRequests.Count();
                int newSplitPinsCount = splitPinRequests.Count();

                if (newWorkoutPinsCount > 0)
                    await workoutPinCreateRangeService.Add(workoutPinRequests.Select((x, i) => new WorkoutPin()
                    {
                        UserId = userId,
                        WorkoutId = x.Id,
                        Order = pins.HighestOrder + i + 1
                    }));

                if (newSplitPinsCount > 0)
                    await splitPinCreateRangeService.Add(splitPinRequests.Select((x, i) => new SplitPin()
                    {
                        UserId = userId,
                        SplitId = x.Id,
                        Order = pins.HighestOrder + i + 1 + newWorkoutPinsCount
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
