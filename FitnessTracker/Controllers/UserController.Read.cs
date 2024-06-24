using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [Authorize]
        [HttpGet("me/detailed")]
        public async Task<IActionResult> GetDetailed()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var user = await readSingleService.Get(x => x.Id == userId, "followers,following,completedworkouts");
            if (user is null)
                return Unauthorized();

            return Ok(detailedResponseMapper.Map(user));
        }
    }
}
