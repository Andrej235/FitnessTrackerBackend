using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Services.Read;
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

            Models.User? user = await readSingleService.Get(
                x => x.Id == userId,
                x => x.Include(x => x.CurrentSplit!)
                      .ThenInclude(x => x.Creator)
                      .Include(x => x.CurrentSplit!)
                      .ThenInclude(x => x.Workouts)
                      .ThenInclude(x => x.Workout));

            if (user is null)
                return Unauthorized();

            DetailedUserResponseDTO mapped = detailedResponseMapper.Map(user);
            mapped.Followers = await followerCountService.Count(x => x.FolloweeId == userId);
            mapped.Following = await followerCountService.Count(x => x.FollowerId == userId);
            mapped.TotalCompletedWorkouts = await completedWorkoutCountService.Count(x => x.UserId == userId);

            return Ok(mapped);
        }

        [Authorize]
        [HttpGet("me/profilepicture")]
        [ProducesResponseType(typeof(UserProfilePictureResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetProfilePic()
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            UserProfilePictureResponseDTO? user = await readSingleSelectedService.Get(
                 x => new UserProfilePictureResponseDTO
                 {
                     Image = x.ProfilePic
                 },
                 x => x.Id == userId);

            if (user is null)
                return Unauthorized();

            return Ok(user);
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
                    ? await followerReadRangeService.Get(x => x.FollowerId == id, offset ?? 0, limit ?? 10, x => x.Include(x => x.Followee))
                    : await followerReadRangeService.Get(x => x.FollowerId == id && EF.Functions.Like(x.Followee.Name, $"{name}%"), offset ?? 0, limit ?? 10, x => x.Include(x => x.Followee));

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
                    ? await followerReadRangeService.Get(x => x.FolloweeId == id, offset ?? 0, limit ?? 10, x => x.Include(x => x.Follower))
                    : await followerReadRangeService.Get(x => x.FolloweeId == id && EF.Functions.Like(x.Follower.Name, $"{name}%"), offset ?? 0, limit ?? 10, x => x.Include(x => x.Follower));

                return Ok(follows.Select(x => simpleResponseMapper.Map(x.Follower)));
            }
            catch (Exception ex)
            {
                ex.LogError();
                return NotFound();
            }
        }

        [HttpGet("{username}/detailed")]
        [ProducesResponseType(typeof(DetailedPublicUserResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetailed(string username)
        {
            Models.User? user = await readSingleService.Get(x => x.Username == username);

            if (user is null)
                return Unauthorized();

            DetailedPublicUserResponseDTO mapped = publicUserDetailedResponseMapper.Map(user);
            mapped.Followers = await followerCountService.Count(x => x.FolloweeId == user.Id);
            mapped.Following = await followerCountService.Count(x => x.FollowerId == user.Id);
            mapped.TotalCompletedWorkouts = await completedWorkoutCountService.Count(x => x.UserId == user.Id);

            if (User.Identity is ClaimsIdentity claimsIdentity
                && claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is string userIdString
                && Guid.TryParse(userIdString, out Guid userId))
            {
                if (userId == user.Id)
                    mapped.IsMe = true;
                else
                    mapped.IsFollowing = (await followerReadSingleService.Get(x => x.FollowerId == userId && x.FolloweeId == user.Id)) is not null;
            }

            return Ok(mapped);
        }

        [HttpGet("{username}/following")]
        [ProducesResponseType(typeof(IEnumerable<SimpleUserResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFollowing(string username, [FromQuery] string? searchTerm, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            try
            {
                Models.User? user = await readSingleService.Get(
                    x => x.Username == username,
                    x => x.Include(x => x.Settings));

                if (user is null)
                    return NotFound();

                if (!user.Settings.PublicFollowing)
                    return Forbid();

                IEnumerable<Models.UserFollows> follows = searchTerm is null
                    ? await followerReadRangeService.Get(x => x.FollowerId == user.Id, offset ?? 0, limit ?? 10, x => x.Include(x => x.Followee))
                    : await followerReadRangeService.Get(x => x.FollowerId == user.Id && EF.Functions.Like(x.Followee.Name, $"{searchTerm}%"), offset ?? 0, limit ?? 10, x => x.Include(x => x.Followee));

                return Ok(follows.Select(x => simpleResponseMapper.Map(x.Followee)));
            }
            catch (Exception ex)
            {
                ex.LogError();
                return NotFound();
            }
        }

        [HttpGet("{username}/followers")]
        [ProducesResponseType(typeof(IEnumerable<SimpleUserResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFollowers(string username, [FromQuery] string? searchTerm, [FromQuery] int? limit, [FromQuery] int? offset)
        {
            try
            {
                Models.User? user = await readSingleService.Get(
                    x => x.Username == username,
                    x => x.Include(x => x.Settings));

                if (user is null)
                    return NotFound();

                if (!user.Settings.PublicFollowing)
                    return Forbid();

                IEnumerable<Models.UserFollows> follows = searchTerm is null
                    ? await followerReadRangeService.Get(x => x.FolloweeId == user.Id, offset ?? 0, limit ?? 10, x => x.Include(x => x.Follower))
                    : await followerReadRangeService.Get(x => x.FolloweeId == user.Id && EF.Functions.Like(x.Follower.Name, $"{searchTerm}%"), offset ?? 0, limit ?? 10, x => x.Include(x => x.Follower));

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
