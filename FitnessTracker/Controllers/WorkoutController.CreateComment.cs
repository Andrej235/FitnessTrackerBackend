using FitnessTracker.DTOs.Requests.Workout;
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
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateComment(Guid workoutId, [FromBody] CreateWorkoutCommentRequestDTO request)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            try
            {
                Models.WorkoutComment mapped = commentCreateRequestMapper.Map(request);
                mapped.CreatorId = userId;
                mapped.WorkoutId = workoutId;

                Models.WorkoutComment newComment = await commentCreateService.Add(mapped);
                return Created($"workouts/{workoutId}/comment/{newComment.Id}", newComment.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetErrorMessage());
            }
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPost("{workoutId:guid}/comment/{commentId:guid}/reply")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateReply(Guid workoutId, Guid commentId, [FromBody] CreateWorkoutCommentRequestDTO request)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            try
            {
                Models.WorkoutComment mapped = commentCreateRequestMapper.Map(request);
                mapped.CreatorId = userId;
                mapped.WorkoutId = workoutId;
                mapped.ParentId = commentId;

                Models.WorkoutComment newComment = await commentCreateService.Add(mapped);
                return Created($"workouts/{workoutId}/comment/{newComment.Id}", newComment.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.GetErrorMessage());
            }
        }
    }
}
