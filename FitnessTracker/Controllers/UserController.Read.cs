using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [Authorize]
        [HttpGet("me/detailed")]
        [ProducesResponseType(typeof(DetailedUserResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetDetailed()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var user = await readSingleService.Get(x => x.Id == userId, "detailed");
            return user is null ? Unauthorized() : Ok(detailedResponseMapper.Map(user));
        }

        [HttpGet("{id}/detailed")]
        [ProducesResponseType(typeof(DetailedPublicUserResponseDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailed(Guid id)
        {
            var user = await readSingleService.Get(x => x.Id == id, "detailed");
            if (user is null)
                return Unauthorized();

            var mapped = publicUserDetailedResponseMapper.Map(user);

            if (User.Identity is ClaimsIdentity claimsIdentity
                && claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is string userIdString
                && Guid.TryParse(userIdString, out var userId))
            {
                if (userId == id)
                    mapped.IsMe = true;
                else
                    mapped.IsFollowing = user.Followers.Any(x => x.FollowerId == userId);
            }

            return Ok(mapped);
        }

        [HttpGet("{id}/following")]
        [ProducesResponseType(typeof(IEnumerable<SimpleUserResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFollowing(Guid id, [FromQuery] string? name, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            try
            {
                var follows = name is null
                    ? await followerReadRangeService.Get(x => x.FollowerId == id, offset ?? 0, limit ?? 10, "followee")
                    : await followerReadRangeService.Get(x => x.FollowerId == id && EF.Functions.Like(x.Followee.Name, $"{name}%"), offset ?? 0, limit ?? 10, "followee");

                return Ok(follows.Select(x => simpleResponseMapper.Map(x.Followee)));
            }
            catch (Exception ex)
            {
                ex.LogError();
                return NotFound();
            }
        }

        [HttpGet("{id}/followers")]
        [ProducesResponseType(typeof(IEnumerable<SimpleUserResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFollowers(Guid id, [FromQuery] string? name, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            try
            {
                var follows = name is null
                    ? await followerReadRangeService.Get(x => x.FolloweeId == id, offset ?? 0, limit ?? 10, "follower")
                    : await followerReadRangeService.Get(x => x.FolloweeId == id && EF.Functions.Like(x.Follower.Name, $"{name}%"), offset ?? 0, limit ?? 10, "follower");

                return Ok(follows.Select(x => simpleResponseMapper.Map(x.Follower)));
            }
            catch (Exception ex)
            {
                ex.LogError();
                return NotFound();
            }
        }

        [Authorize]
        [HttpGet("me/following")]
        [ProducesResponseType(typeof(IEnumerable<SimpleUserResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFollowing([FromQuery] string? name, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out var id))
                    return Unauthorized();

                var follows = name is null
                    ? await followerReadRangeService.Get(x => x.FollowerId == id, offset ?? 0, limit ?? 10, "followee")
                    : await followerReadRangeService.Get(x => x.FollowerId == id && EF.Functions.Like(x.Followee.Name, $"{name}%"), offset ?? 0, limit ?? 10, "followee");

                return Ok(follows.Select(x => simpleResponseMapper.Map(x.Followee)));
            }
            catch (Exception ex)
            {
                ex.LogError();
                return NotFound();
            }
        }

        [Authorize]
        [HttpGet("me/followers")]
        [ProducesResponseType(typeof(IEnumerable<SimpleUserResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFollowers([FromQuery] string? name, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            try
            {
                if (User.Identity is not ClaimsIdentity claimsIdentity
                    || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                    || !Guid.TryParse(userIdString, out var id))
                    return Unauthorized();

                var follows = name is null
                    ? await followerReadRangeService.Get(x => x.FolloweeId == id, offset ?? 0, limit ?? 10, "follower")
                    : await followerReadRangeService.Get(x => x.FolloweeId == id && EF.Functions.Like(x.Follower.Name, $"{name}%"), offset ?? 0, limit ?? 10, "follower");

                return Ok(follows.Select(x => simpleResponseMapper.Map(x.Follower)));
            }
            catch (Exception ex)
            {
                ex.LogError();
                return NotFound();
            }
        }
    }
}
