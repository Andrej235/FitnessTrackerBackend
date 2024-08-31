using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.Services.Read.Full;
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
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            IEnumerable<Models.SplitComment> comments = await commentReadRangeService.Get(
                x => x.SplitId == splitId && x.ParentId == null,
                offset,
                limit ?? 10,
                x => x.Include(x => x.Creator).Include(x => x.Likes));

            IEnumerable<SimpleSplitCommentResponseDTO> mapped = comments.Select(x =>
            {
                SimpleSplitCommentResponseDTO mapped = simpleCommentResponseMapper.Map(x);
                mapped.IsLiked = x.Likes.Any(x => x.Id == userId);
                mapped.IsCreator = x.Creator.Id == userId;
                return mapped;
            });
            return Ok(mapped);
        }

        [Authorize(Roles = $"{Role.Admin},{Role.User}")]
        [HttpGet("{splitId:guid}/comment/{commentId:guid}/reply")]
        [ProducesResponseType(typeof(IEnumerable<SimpleSplitCommentResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetReplies(Guid splitId, Guid commentId, [FromQuery] int? offset, [FromQuery] int? limit)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            IEnumerable<Models.SplitComment> comments = await commentReadRangeService.Get(
                x => x.SplitId == splitId && x.ParentId == commentId,
                offset,
                limit ?? 10,
                x => x.Include(x => x.Creator).Include(x => x.Likes));

            IEnumerable<SimpleSplitCommentResponseDTO> mapped = comments.Select(x =>
            {
                SimpleSplitCommentResponseDTO mapped = simpleCommentResponseMapper.Map(x);
                mapped.IsLiked = x.Likes.Any(x => x.Id == userId);
                mapped.IsCreator = x.Creator.Id == userId;
                return mapped;
            });
            return Ok(mapped);
        }
    }
}
