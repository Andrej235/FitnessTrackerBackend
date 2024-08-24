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

                object? newId = await commentCreateService.Add(mapped);
                return newId == default ? BadRequest("Failed to create comment") : Created($"workouts/{workoutId}/comment/{newId}", newId);
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest("Failed to create comment");
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

                object? newId = await commentCreateService.Add(mapped);
                return newId == default ? BadRequest("Failed to create comment") : Created($"workouts/{workoutId}/comment/{newId}", newId);
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest("Failed to create comment");
            }
        }
    }
}
