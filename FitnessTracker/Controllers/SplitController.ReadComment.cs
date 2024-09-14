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
        [HttpGet("{splitId:guid}/comment")]
        [ProducesResponseType(typeof(IEnumerable<SimpleSplitCommentResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetComments(Guid splitId, [FromQuery] int? offset, [FromQuery] int? limit)
        {
            Guid userId = default;
            if (User.Identity is ClaimsIdentity claimsIdentity
                && claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is string userIdString)
                _ = Guid.TryParse(userIdString, out userId);

            return Ok(await splitService.GetComments(splitId, userId, offset, limit));
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("{splitId:guid}/comment/{commentId:guid}/reply")]
        [ProducesResponseType(typeof(IEnumerable<SimpleSplitCommentResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetReplies(Guid splitId, Guid commentId, [FromQuery] int? offset, [FromQuery] int? limit)
        {
            Guid userId = default;
            if (User.Identity is ClaimsIdentity claimsIdentity
                && claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is string userIdString)
                _ = Guid.TryParse(userIdString, out userId);

            return Ok(await splitService.GetReplies(splitId, commentId, userId, offset, limit));
        }
    }
}
