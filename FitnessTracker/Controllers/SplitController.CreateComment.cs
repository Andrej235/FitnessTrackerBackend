using FitnessTracker.DTOs.Requests.Split;
using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class SplitController
    {
        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPost("{splitId:guid}/comment")]
        [ProducesResponseType(typeof(NewSplitCommentResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateComment(Guid splitId, [FromBody] CreateSplitCommentRequestDTO request)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            Guid newCommentId = await splitService.CreateComment(splitId, userId, request);
            return Created((string?)null, new NewSplitCommentResponseDTO()
            {
                NewCommentId = newCommentId
            });
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpPost("{splitId:guid}/comment/{commentId:guid}/reply")]
        [ProducesResponseType(typeof(NewSplitCommentResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateComment(Guid splitId, Guid commentId, [FromBody] CreateSplitCommentRequestDTO request)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            Guid newCommentId = await splitService.CreateReply(splitId, commentId, userId, request);
            return Created((string?)null, new NewSplitCommentResponseDTO()
            {
                NewCommentId = newCommentId
            });
        }
    }
}
