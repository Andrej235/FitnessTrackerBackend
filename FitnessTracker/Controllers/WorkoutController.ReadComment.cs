using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Services.Read;
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

            IEnumerable<object> comments = await commentSelectService.Get(
                x => new SimpleWorkoutCommentResponseDTO
                {
                    Id = x.Id,
                    Text = x.Text,
                    Creator = new DTOs.Responses.User.SimpleUserResponseDTO
                    {
                        Username = x.Creator.Username,
                        Name = x.Creator.Name,
                        Image = x.Creator.ProfilePic,
                    },
                    CreatedAt = x.CreatedAt,
                    LikeCount = x.Likes.Count,
                    IsLiked = x.Likes.Any(x => x.Id == userId),
                    IsCreator = x.CreatorId == userId,
                    ReplyCount = x.Children.Count,
                    WorkoutId = x.WorkoutId,
                },
                x => x.WorkoutId == workoutId && x.ParentId == null,
                offset ?? 0,
                limit ?? 10,
                x => x.Include(x => x.Creator));

            return Ok(comments);
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

            IEnumerable<object> comments = await commentSelectService.Get(
                x => new SimpleWorkoutCommentResponseDTO
                {
                    Id = x.Id,
                    Text = x.Text,
                    Creator = new DTOs.Responses.User.SimpleUserResponseDTO
                    {
                        Username = x.Creator.Username,
                        Name = x.Creator.Name,
                        Image = x.Creator.ProfilePic,
                    },
                    CreatedAt = x.CreatedAt,
                    LikeCount = x.Likes.Count,
                    IsLiked = x.Likes.Any(x => x.Id == userId),
                    IsCreator = x.CreatorId == userId,
                    ReplyCount = x.Children.Count,
                    WorkoutId = x.WorkoutId,
                },
                x => x.WorkoutId == workoutId && x.ParentId == commentId,
                offset ?? 0,
                limit ?? 10,
                x => x.Include(x => x.Creator));

            return Ok(comments);
        }
    }
}
