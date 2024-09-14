using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public partial class UserService
    {
        public async Task<DetailedUserResponseDTO> GetDetailed(Guid userId)
        {
            if (userId == default)
                throw new UnauthorizedException();

            User? user = await readSingleService.Get(
                x => x.Id == userId,
                x => x.Include(x => x.CurrentSplit!)
                      .ThenInclude(x => x.Creator)
                      .Include(x => x.CurrentSplit!)
                      .ThenInclude(x => x.Workouts)
                      .ThenInclude(x => x.Workout))
                ?? throw new UnauthorizedException();

            DetailedUserResponseDTO mapped = detailedResponseMapper.Map(user);
            mapped.Followers = await followerCountService.Count(x => x.FolloweeId == userId);
            mapped.Following = await followerCountService.Count(x => x.FollowerId == userId);
            mapped.TotalCompletedWorkouts = await completedWorkoutCountService.Count(x => x.UserId == userId);

            return mapped;
        }

        public async Task<UserProfilePictureResponseDTO> GetProfilePic(Guid userId)
        {
            if (userId == default)
                throw new UnauthorizedException();

            UserProfilePictureResponseDTO? user = await readSingleSelectedService.Get(
                x => new UserProfilePictureResponseDTO
                {
                    Image = x.ProfilePic
                },
                x => x.Id == userId)
                ?? throw new UnauthorizedException();

            return user;
        }

        public async Task<DetailedPublicUserResponseDTO> GetDetailed(string username, Guid? userId)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new InvalidArgumentException("Username cannot be empty");

            User? user = await readSingleService.Get(x => x.Username == username) ?? throw new NotFoundException($"User '{username}' not found");

            DetailedPublicUserResponseDTO mapped = publicUserDetailedResponseMapper.Map(user);
            mapped.Followers = await followerCountService.Count(x => x.FolloweeId == user.Id);
            mapped.Following = await followerCountService.Count(x => x.FollowerId == user.Id);
            mapped.TotalCompletedWorkouts = await completedWorkoutCountService.Count(x => x.UserId == user.Id);

            if (userId == user.Id)
                mapped.IsMe = true;
            else
                mapped.IsFollowing = (await followerReadSingleService.Get(x => x.FollowerId == userId && x.FolloweeId == user.Id)) is not null;

            return mapped;
        }

        public async Task<IEnumerable<SimpleUserResponseDTO>> GetFollowers(Guid userId, string? nameFilter, int? offset, int? limit)
        {
            if (userId == default)
                throw new UnauthorizedException();

            IEnumerable<UserFollows> follows = nameFilter is null
                ? await followerReadRangeService.Get(x => x.FolloweeId == userId, offset ?? 0, limit ?? 10, x => x.Include(x => x.Follower))
                : await followerReadRangeService.Get(x => x.FolloweeId == userId && EF.Functions.Like(x.Follower.Name, $"{nameFilter}%"), offset ?? 0, limit ?? 10, x => x.Include(x => x.Follower));

            return follows.Select(x => simpleResponseMapper.Map(x.Follower));
        }

        public async Task<IEnumerable<SimpleUserResponseDTO>> GetFollowersFor(string username, string? nameFilter, int? offset, int? limit)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new InvalidArgumentException("Username cannot be empty");

            User user = await readSingleService.Get(
                x => x.Username == username,
                x => x.Include(x => x.Settings))
                ?? throw new NotFoundException($"User '{username}' not found");

            if (!user.Settings.PublicFollowing)
                throw new AccessDeniedException("User's following is not public");

            IEnumerable<UserFollows> follows = nameFilter is null
                ? await followerReadRangeService.Get(x => x.FolloweeId == user.Id, offset ?? 0, limit ?? 10, x => x.Include(x => x.Follower))
                : await followerReadRangeService.Get(x => x.FolloweeId == user.Id && EF.Functions.Like(x.Follower.Name, $"{nameFilter}%"), offset ?? 0, limit ?? 10, x => x.Include(x => x.Follower));

            return follows.Select(x => simpleResponseMapper.Map(x.Follower));
        }

        public async Task<IEnumerable<SimpleUserResponseDTO>> GetFollowing(Guid userId, string? nameFilter, int? offset, int? limit)
        {
            if (userId == default)
                throw new UnauthorizedException();

            IEnumerable<UserFollows> follows = nameFilter is null
                ? await followerReadRangeService.Get(x => x.FollowerId == userId, offset ?? 0, limit ?? 10, x => x.Include(x => x.Followee))
                : await followerReadRangeService.Get(x => x.FollowerId == userId && EF.Functions.Like(x.Followee.Name, $"{nameFilter}%"), offset ?? 0, limit ?? 10, x => x.Include(x => x.Followee));

            return follows.Select(x => simpleResponseMapper.Map(x.Followee));
        }

        public async Task<IEnumerable<SimpleUserResponseDTO>> GetFollowingFor(string username, string? nameFilter, int? offset, int? limit)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new InvalidArgumentException("Username cannot be empty");

            User? user = await readSingleService.Get(
                x => x.Username == username,
                x => x.Include(x => x.Settings))
                ?? throw new NotFoundException($"User '{username}' not found");

            if (!user.Settings.PublicFollowing)
                throw new AccessDeniedException("User's following is not public");

            IEnumerable<Models.UserFollows> follows = nameFilter is null
                ? await followerReadRangeService.Get(x => x.FollowerId == user.Id, offset ?? 0, limit ?? 10, x => x.Include(x => x.Followee))
                : await followerReadRangeService.Get(x => x.FollowerId == user.Id && EF.Functions.Like(x.Followee.Name, $"{nameFilter}%"), offset ?? 0, limit ?? 10, x => x.Include(x => x.Followee));

            return follows.Select(x => simpleResponseMapper.Map(x.Followee));
        }
    }
}
