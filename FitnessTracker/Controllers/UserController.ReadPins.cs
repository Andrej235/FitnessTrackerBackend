using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Services.Read;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [Authorize]
        [HttpGet("me/pins")]
        [ProducesResponseType(typeof(IEnumerable<SimplePinResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPins()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            var pins = await readSingleSelectedService.Get(x => new
            {
                Workout = x.WorkoutPins,
                Split = x.SplitPins,
            },
            x => x.Id == userId,
            x => x.Include(x => x.WorkoutPins).Include(x => x.SplitPins));

            if (pins is null)
                return NotFound();

            return Ok(pins.Workout.Select(workoutPinResponseMapper.Map).Union(pins.Split.Select(splitPinResponseMapper.Map)));
        }

        [Authorize]
        [HttpGet("{username}/pins")]
        [ProducesResponseType(typeof(IEnumerable<SimplePinResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPins(string username)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            var pins = await readSingleSelectedService.Get(x => new
            {
                Workout = x.WorkoutPins,
                Split = x.SplitPins,
            },
            x => x.Username == username,
            x => x.Include(x => x.WorkoutPins).Include(x => x.SplitPins));

            if (pins is null)
                return NotFound();

            return Ok(pins.Workout.Select(workoutPinResponseMapper.Map).Union(pins.Split.Select(splitPinResponseMapper.Map)));
        }
    }
}
