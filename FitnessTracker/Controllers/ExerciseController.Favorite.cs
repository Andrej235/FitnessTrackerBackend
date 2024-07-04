using FitnessTracker.Models;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class ExerciseController
    {
        [Authorize]
        [HttpPost("{id}/favorite")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Favorite(int id)
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out var userId))
                    return Unauthorized();

                await favoriteExerciseCreateService.Add(new FavoriteExercise
                {
                    UserId = userId,
                    ExerciseId = id
                });
                return Created();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest("Failed to add favorite");
            }
        }

        [Authorize]
        [HttpDelete("{id}/favorite")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Unfavorite(int id)
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out var userId))
                    return Unauthorized();

                await favoriteExerciseDeleteService.Delete(x => x.UserId == userId && x.ExerciseId == id);
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
