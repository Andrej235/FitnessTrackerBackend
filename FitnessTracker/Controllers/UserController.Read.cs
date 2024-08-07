﻿using FitnessTracker.DTOs.Responses.User;
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
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            Models.User? user = await readSingleService.Get(x => x.Id == userId, "detailed");
            if (user is null)
                return Unauthorized();

            DetailedUserResponseDTO mapped = detailedResponseMapper.Map(user);
            mapped.Followers = await followerCountService.Count(x => x.FolloweeId == userId);
            mapped.Following = await followerCountService.Count(x => x.FollowerId == userId);
            mapped.TotalCompletedWorkouts = await completedWorkoutCountService.Count(x => x.UserId == userId);

            return Ok(mapped);
        }

        [HttpGet("{id}/detailed")]
        [ProducesResponseType(typeof(DetailedPublicUserResponseDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDetailed(Guid id)
        {
            Models.User? user = await readSingleService.Get(x => x.Id == id, "detailed");
            if (user is null)
                return Unauthorized();

            DetailedPublicUserResponseDTO mapped = publicUserDetailedResponseMapper.Map(user);
            mapped.Followers = await followerCountService.Count(x => x.FolloweeId == id);
            mapped.Following = await followerCountService.Count(x => x.FollowerId == id);
            mapped.TotalCompletedWorkouts = await completedWorkoutCountService.Count(x => x.UserId == id);

            if (User.Identity is ClaimsIdentity claimsIdentity
                && claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is string userIdString
                && Guid.TryParse(userIdString, out Guid userId))
            {
                if (userId == id)
                    mapped.IsMe = true;
                else
                    mapped.IsFollowing = (await followerReadSingleService.Get(x => x.FollowerId == userId && x.FolloweeId == id)) is not null;
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
                IEnumerable<Models.UserFollows> follows = name is null
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
                IEnumerable<Models.UserFollows> follows = name is null
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
                    || !Guid.TryParse(userIdString, out Guid id))
                    return Unauthorized();

                IEnumerable<Models.UserFollows> follows = name is null
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
                    || !Guid.TryParse(userIdString, out Guid id))
                    return Unauthorized();

                IEnumerable<Models.UserFollows> follows = name is null
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
