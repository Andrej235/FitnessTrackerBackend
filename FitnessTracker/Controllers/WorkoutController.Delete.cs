using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class WorkoutController
    {
        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            try
            {
                _ = await likeDeleteRangeService.Delete(x => x.WorkoutId == id);
                _ = await favoriteDeleteRangeService.Delete(x => x.WorkoutId == id);
                _ = await commentLikeDeleteRangeService.Delete(x => x.WorkoutId == id);
                _ = await commentDeleteRangeService.Delete(x => x.WorkoutId == id);
                await deleteService.Delete(x => x.CreatorId == userId && x.Id == id);
                return NoContent();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return NotFound();
            }
        }
    }
}
