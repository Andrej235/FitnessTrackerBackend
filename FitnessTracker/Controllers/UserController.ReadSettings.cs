using FitnessTracker.Services.Read;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [Authorize]
        [HttpGet("me/settings")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetSettings()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            Models.UserSettings? settings = await settingsReadSingleService.Get(x => x.UserId == userId, x => x.AsNoTracking());

            if (settings is null)
                return Unauthorized();

            return Ok(settingsResponseMapper.Map(settings));
        }
    }
}
