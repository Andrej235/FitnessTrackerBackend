using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class SplitController
    {

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPost("{id:guid}/favorite")]
        public async Task<IActionResult> CreateFavorite(Guid id)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            try
            {
                await favoriteCreateService.Add(new()
                {
                    UserId = userId,
                    SplitId = id
                });
                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest("Failed to favorite");
            }
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpDelete("{id:guid}/favorite")]
        public async Task<IActionResult> DeleteFavorite(Guid id)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            try
            {
                await favoriteDeleteService.Delete(x => x.UserId == userId && x.SplitId == id);
                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest("Failed to remove favorite");
            }
        }
    }
}
