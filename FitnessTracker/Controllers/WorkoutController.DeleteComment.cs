using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class WorkoutController
    {
        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpDelete("{workoutId:guid}/comment/{commentId:guid}")]
        public async Task<IActionResult> DeleteComment(Guid workoutId, Guid commentId)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            try
            {
                await commentDeleteService.Delete(x => x.WorkoutId == workoutId && x.CreatorId == userId && x.Id == commentId);
                await commentDeleteRangeService.Delete(x => x.WorkoutId == workoutId && x.ParentId == commentId);
                return Ok();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest("Failed to delete comment");
            }
        }
    }
}
