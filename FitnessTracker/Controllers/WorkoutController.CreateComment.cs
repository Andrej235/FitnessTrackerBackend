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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateComment(Guid workoutId, [FromBody] CreateWorkoutCommentRequestDTO request)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            try
            {
                var mapped = createCommentRequestMapper.Map(request);
                mapped.CreatorId = userId;
                mapped.WorkoutId = workoutId;

                var newId = await commentCreateService.Add(mapped);
                if (newId == default)
                    return BadRequest("Failed to create comment");

                return Created();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest("Failed to create comment");
            }
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPost("{workoutId:guid}/comment/{parentId:guid}/reply")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateComment(Guid workoutId, Guid parentId, [FromBody] CreateWorkoutCommentRequestDTO request)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            try
            {
                var mapped = createCommentRequestMapper.Map(request);
                mapped.CreatorId = userId;
                mapped.WorkoutId = workoutId;
                mapped.ParentId = parentId;

                var newId = await commentCreateService.Add(mapped);
                if (newId == default)
                    return BadRequest("Failed to create comment");

                return Created();
            }
            catch (Exception ex)
            {
                ex.LogError();
                return BadRequest("Failed to create comment");
            }
        }
    }
}
