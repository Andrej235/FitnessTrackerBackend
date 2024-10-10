using FitnessTracker.DTOs.Requests.Workout;
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
        [HttpPost("{workoutId:guid}/comment")]
        [ProducesResponseType(typeof(NewWorkoutCommentResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateComment(Guid workoutId, [FromBody] CreateWorkoutCommentRequestDTO request)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            Guid newCommentId = await workoutService.CreateComment(userId, workoutId, request);
            return Created((string?)null, new NewWorkoutCommentResponseDTO()
            {
                NewCommentId = newCommentId
            });
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPost("{workoutId:guid}/comment/{commentId:guid}/reply")]
        [ProducesResponseType(typeof(NewWorkoutCommentResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateReply(Guid workoutId, Guid commentId, [FromBody] CreateWorkoutCommentRequestDTO request)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            Guid newCommentId = await workoutService.CreateReply(userId, workoutId, commentId, request);
            return Created((string?)null, new NewWorkoutCommentResponseDTO()
            {
                NewCommentId = newCommentId
            });
        }
    }
}
