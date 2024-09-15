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
                return Ok(await workoutService.GetComments(workoutId, null, offset, limit));
            else
                return Ok(await workoutService.GetComments(workoutId, userId, offset, limit));
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
                return Ok(await workoutService.GetReplies(workoutId, commentId, null, offset, limit));
            else
                return Ok(await workoutService.GetReplies(workoutId, commentId, userId, offset, limit));
        }
    }
}
