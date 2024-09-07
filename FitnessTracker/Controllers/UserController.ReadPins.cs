using FitnessTracker.DTOs.Responses.Pins;
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
        [ProducesResponseType(typeof(IEnumerable<PinResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetPins()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            var pins = await readSingleSelectedService.Get(x => new
            {
                WorkoutPins = x.WorkoutPins.Select(x => new
                {
                    Pin = x,
                    LikeCount = x.Workout.Likes.Count,
                }),
                SplitPins = x.SplitPins.Select(x => new
                {
                    Pin = x,
                    LikeCount = x.Split.Likes.Count,
                }),
            },
            x => x.Id == userId,
            x => x.Include(x => x.WorkoutPins)
                .ThenInclude(x => x.Workout)
                .Include(x => x.SplitPins)
                .ThenInclude(x => x.Split));

            if (pins is null)
                return Unauthorized();

            IEnumerable<PinResponseDTO> mappedWorkoutPins = pins.WorkoutPins.Select(x =>
            {
                PinResponseDTO mapped = workoutPinResponseMapper.Map(x.Pin);
                mapped.LikeCount = x.LikeCount;
                return mapped;
            });

            IEnumerable<PinResponseDTO> mappedSplitPins = pins.SplitPins.Select(x =>
            {
                PinResponseDTO mapped = splitPinResponseMapper.Map(x.Pin);
                mapped.LikeCount = x.LikeCount;
                return mapped;
            });

            return Ok(mappedWorkoutPins.Union(mappedSplitPins).OrderBy(x => x.Order));
        }

        [HttpGet("{username}/pins")]
        [ProducesResponseType(typeof(IEnumerable<PinResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPins(string username)
        {
            var pins = await readSingleSelectedService.Get(x => new
            {
                WorkoutPins = x.WorkoutPins.Select(x => new
                {
                    Pin = x,
                    LikeCount = x.Workout.Likes.Count,
                }),
                SplitPins = x.SplitPins.Select(x => new
                {
                    Pin = x,
                    LikeCount = x.Split.Likes.Count,
                }),
            },
            x => x.Username == username,
            x => x.Include(x => x.WorkoutPins)
                .ThenInclude(x => x.Workout)
                .Include(x => x.SplitPins)
                .ThenInclude(x => x.Split));

            if (pins is null)
                return NotFound();

            IEnumerable<PinResponseDTO> mappedWorkoutPins = pins.WorkoutPins.Select(x =>
            {
                PinResponseDTO mapped = workoutPinResponseMapper.Map(x.Pin);
                mapped.LikeCount = x.LikeCount;
                return mapped;
            });

            IEnumerable<PinResponseDTO> mappedSplitPins = pins.SplitPins.Select(x =>
            {
                PinResponseDTO mapped = splitPinResponseMapper.Map(x.Pin);
                mapped.LikeCount = x.LikeCount;
                return mapped;
            });

            return Ok(mappedWorkoutPins.Union(mappedSplitPins).OrderBy(x => x.Order));
        }
    }
}
