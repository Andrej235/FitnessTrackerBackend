using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class WorkoutController
    {
        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("{workoutId:guid}/comment")]
        public async Task<IActionResult> GetComments(Guid workoutId)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var comments = await commentReadRangeService.Get(x => x.WorkoutId == workoutId && x.ParentId == null, 0, 10, "creator,likes");
            var mapped = comments.Select(x =>
            {
                var mapped = simpleCommentResponseMapper.Map(x);
                mapped.IsLiked = x.Likes.Any(x => x.Id == userId);
                return mapped;
            });
            return Ok(mapped);
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("{workoutId:guid}/comment/{commentId:guid}/reply")]
        public async Task<IActionResult> GetReplies(Guid workoutId, Guid commentId)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var comments = await commentReadRangeService.Get(x => x.WorkoutId == workoutId && x.ParentId == commentId, 0, 10, "creator,likes");
            var mapped = comments.Select(x =>
            {
                var mapped = simpleCommentResponseMapper.Map(x);
                mapped.IsLiked = x.Likes.Any(x => x.Id == userId);
                return mapped;
            });
            return Ok(mapped);
        }
    }
}
