using FitnessTracker.DTOs.Responses.Workout;
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
        [ProducesResponseType(typeof(IEnumerable<SimpleWorkoutCommentResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetComments(Guid workoutId, [FromQuery] int? offset, [FromQuery] int? limit)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            IEnumerable<Models.WorkoutComment> comments = await commentReadRangeService.Get(x => x.WorkoutId == workoutId && x.ParentId == null, offset ?? 0, limit ?? 10, "creator,likes,children");
            IEnumerable<SimpleWorkoutCommentResponseDTO> mapped = comments.Select(x =>
            {
                SimpleWorkoutCommentResponseDTO mapped = simpleCommentResponseMapper.Map(x);
                mapped.IsLiked = x.Likes.Any(x => x.Id == userId);
                mapped.IsCreator = x.Creator.Id == userId;
                return mapped;
            });
            return Ok(mapped);
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("{workoutId:guid}/comment/{commentId:guid}/reply")]
        [ProducesResponseType(typeof(IEnumerable<SimpleWorkoutCommentResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetReplies(Guid workoutId, Guid commentId, [FromQuery] int? offset, [FromQuery] int? limit)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            IEnumerable<Models.WorkoutComment> comments = await commentReadRangeService.Get(x => x.WorkoutId == workoutId && x.ParentId == commentId, offset ?? 0, limit ?? 10, "creator,likes");
            IEnumerable<SimpleWorkoutCommentResponseDTO> mapped = comments.Select(x =>
            {
                SimpleWorkoutCommentResponseDTO mapped = simpleCommentResponseMapper.Map(x);
                mapped.IsLiked = x.Likes.Any(x => x.Id == userId);
                mapped.IsCreator = x.Creator.Id == userId;
                return mapped;
            });
            return Ok(mapped);
        }
    }
}
