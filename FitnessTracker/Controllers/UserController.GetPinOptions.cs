using FitnessTracker.DTOs.Responses.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [Authorize]
        [HttpGet("me/pins/options")]
        [ProducesResponseType(typeof(IEnumerable<SimplePinOptionResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetPinOptions()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            IEnumerable<SimplePinOptionResponseDTO>? pins = await readSingleSelectedService.Get(x =>
                x.CreatedWorkouts.Where(x => x.IsPublic).Select(x => new SimplePinOptionResponseDTO
                {
                    Name = x.Name,
                    LikeCount = x.Likes.Count,
                    Type = SimplePinResponseDTO.PinType.Workout,
                })
                .Union(x.CreatedSplits.Where(x => x.IsPublic).Select(x => new SimplePinOptionResponseDTO
                {
                    Name = x.Name,
                    LikeCount = x.Likes.Count,
                    Type = SimplePinResponseDTO.PinType.Split,
                }))
                .OrderByDescending(x => x.LikeCount)
                .ToList()
            ,
            x => x.Id == userId);

            if (pins is null)
                return NotFound();

            return Ok(pins);
        }
    }
}
