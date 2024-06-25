using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class SplitController
    {
        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpDelete("{splitId:guid}/comment/{commentId:guid}")]
        public async Task<IActionResult> DeleteComment(Guid splitId, Guid commentId)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            try
            {
                await commentDeleteRangeService.Delete(x => x.SplitId == splitId && x.ParentId == commentId);
                await commentDeleteService.Delete(x => x.SplitId == splitId && x.CreatorId == userId && x.Id == commentId);
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
