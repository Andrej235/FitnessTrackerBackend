using FitnessTracker.DTOs.Requests.Pins;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [Authorize]
        [HttpPatch("me/pins/reorder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ReorderPins([FromBody] ReorderPinsRequestDTO request)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            var pins = await readSingleSelectedService.Get(x => new
            {
                x.WorkoutPins,
                x.SplitPins
            },
            x => x.Id == userId,
            x => x.Include(x => x.WorkoutPins).Include(x => x.SplitPins));

            if (pins is null)
                return Unauthorized();

            if (pins.WorkoutPins.Count == 0 && pins.SplitPins.Count == 0)
                return BadRequest();

            IEnumerable<UpdateSinglePinRequestDTO> workoutRequests = request.NewOrder.Where(x => x.Type == DTOs.Enums.PinType.Workout);
            IEnumerable<UpdateSinglePinRequestDTO> splitRequests = request.NewOrder.Where(x => x.Type == DTOs.Enums.PinType.Split);

            if (workoutRequests.Count() != pins.WorkoutPins.Count || splitRequests.Count() != pins.SplitPins.Count)
                return BadRequest();

            if (request.NewOrder.Any(x => x.NewOrder < 0))
                return BadRequest();

            if (request.NewOrder.Any(x => x.NewOrder > pins.WorkoutPins.Count + pins.SplitPins.Count))
                return BadRequest();

            ICollection<WorkoutPin> workoutPins = pins.WorkoutPins;
            ICollection<SplitPin> splitPins = pins.SplitPins;

            List<WorkoutPin> workoutPinsToUpdate = [];
            List<SplitPin> splitPinsToUpdate = [];

            try
            {
                foreach (WorkoutPin pin in workoutPins)
                {
                    int newOrder = workoutRequests.First(x => x.Id == pin.WorkoutId).NewOrder;
                    if (newOrder == pin.Order)
                        continue;

                    pin.Order = newOrder;
                    workoutPinsToUpdate.Add(pin);
                }

                foreach (SplitPin pin in splitPins)
                {
                    int newOrder = splitRequests.First(x => x.Id == pin.SplitId).NewOrder;
                    if (newOrder == pin.Order)
                        continue;

                    pin.Order = newOrder;
                    splitPinsToUpdate.Add(pin);
                }

                await workoutPinUpdateRangeService.Update(workoutPinsToUpdate);
                await splitPinUpdateRangeService.Update(splitPinsToUpdate);
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest();
            }

            return NoContent();
        }
    }
}
