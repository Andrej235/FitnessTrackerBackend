using FitnessTracker.DTOs.Enums;
using FitnessTracker.DTOs.Responses.Pins;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [Authorize]
        [HttpGet("me/pins/options")]
        [ProducesResponseType(typeof(IEnumerable<PinResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetPinOptions()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            IEnumerable<PinResponseDTO>? pins = await readSingleSelectedService.Get(x =>
                x.CreatedWorkouts.Where(x => x.IsPublic).Select(x => new PinResponseDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    LikeCount = x.Likes.Count,
                    Type = PinType.Workout,
                    Description = x.Description ?? "",
                    Order = -1,
                })
                .Union(x.CreatedSplits.Where(x => x.IsPublic).Select(x => new PinResponseDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    LikeCount = x.Likes.Count,
                    Type = PinType.Split,
                    Description = x.Description ?? "",
                    Order = -1,
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
