using FitnessTracker.Models;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class WorkoutController
    {
        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPost("{id:guid}/like")]
        public async Task<IActionResult> CreateLike(Guid id)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            try
            {
                await likeCreateService.Add(new WorkoutLike
                {
                    UserId = userId,
                    WorkoutId = id
                });
                return Ok();

            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest("Failed to like");
            }
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpDelete("{id:guid}/like")]
        public async Task<IActionResult> DeleteLike(Guid id)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            try
            {
                await likeDeleteService.Delete(x => x.UserId == userId && x.WorkoutId == id);
                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest("Failed to remove like");
            }
        }
    }
}
