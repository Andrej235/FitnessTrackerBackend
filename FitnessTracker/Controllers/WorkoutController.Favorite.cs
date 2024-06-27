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
        [HttpPost("{id:guid}/favorite")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateFavorite(Guid id)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            try
            {
                await favoriteCreateService.Add(new FavoriteWorkout
                {
                    UserId = userId,
                    WorkoutId = id
                });
                return Created();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest("Failed to favorite");
            }
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpDelete("{id:guid}/favorite")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteFavorite(Guid id)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            try
            {
                await favoriteDeleteService.Delete(x => x.UserId == userId && x.WorkoutId == id);
                return NoContent();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest("Failed to remove favorite");
            }
        }
    }
}
