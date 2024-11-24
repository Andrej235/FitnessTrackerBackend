using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Services.ModelServices.WorkoutService
{
    public partial class WorkoutService
    {
        public async Task<IEnumerable<SimpleWorkoutResponseDTO>> GetAllBy(string username, Guid? userId, string? nameFilter, int? limit, int? offset)
        {
            var creator = await userReadSingleSelectedService.Get(
                x => new
                {
                    x.Id,
                    x.Settings.PublicCreatedWorkouts
                },
                x => x.Username == username) ?? throw new NotFoundException($"User {username} not found");

            if (!creator.PublicCreatedWorkouts && userId != creator.Id)
                throw new AccessDeniedException("User's created workouts are private.");

            IEnumerable<Workout> workouts = nameFilter is null
                ? await readRangeService.Get(x => x.CreatorId == creator.Id, offset, limit ?? 10, x => x.Include(x => x.Creator).OrderByDescending(x => x.CreatedAt))
                : await readRangeService.Get(x => x.CreatorId == creator.Id && EF.Functions.Like(x.Name, $"%{nameFilter}%"), offset, limit ?? 10, x => x.Include(x => x.Creator).OrderByDescending(x => x.CreatedAt));

            return workouts.Select(simpleResponseMapper.Map);
        }

        public async Task<IEnumerable<SimpleWorkoutResponseDTO>> GetAllFavoritesBy(string username, Guid? userId, string? nameFilter, int? limit, int? offset)
        {
            var creator = await userReadSingleSelectedService.Get(
                x => new
                {
                    x.Id,
                    x.Settings.PublicFavoriteWorkouts
                },
                x => x.Username == username) ?? throw new NotFoundException($"User {username} not found");

            if (!creator.PublicFavoriteWorkouts && userId != creator.Id)
                throw new AccessDeniedException("User's favorite workouts are private.");

            IEnumerable<FavoriteWorkout> workouts = nameFilter is null
                ? await favoriteReadRangeService.Get(x => x.UserId == creator.Id, offset, limit ?? 10, x => x.Include(x => x.Workout).ThenInclude(x => x.Creator).OrderByDescending(x => x.FavoritedAt))
                : await favoriteReadRangeService.Get(x => x.UserId == creator.Id && EF.Functions.Like(x.Workout.Name, $"%{nameFilter}%"), offset, limit ?? 10, x => x.Include(x => x.Workout).ThenInclude(x => x.Creator).OrderByDescending(x => x.FavoritedAt));

            return workouts.Select(x => simpleResponseMapper.Map(x.Workout));
        }

        public async Task<IEnumerable<SimpleWorkoutResponseDTO>> GetAllLikedBy(string username, Guid? userId, string? nameFilter, int? limit, int? offset)
        {
            var creator = await userReadSingleSelectedService.Get(
                x => new
                {
                    x.Id,
                    x.Settings.PublicLikedWorkouts
                },
                x => x.Username == username) ?? throw new NotFoundException($"User {username} not found");

            if (!creator.PublicLikedWorkouts && userId != creator.Id)
                throw new AccessDeniedException("User's liked workouts are private.");

            IEnumerable<WorkoutLike> workouts = nameFilter is null
                ? await likeReadRangeService.Get(x => x.UserId == creator.Id, offset, limit ?? 10, x => x.Include(x => x.Workout).ThenInclude(x => x.Creator).OrderByDescending(x => x.LikedAt))
                : await likeReadRangeService.Get(x => x.UserId == creator.Id && EF.Functions.Like(x.Workout.Name, $"%{nameFilter}%"), offset, limit ?? 10, x => x.Include(x => x.Workout).ThenInclude(x => x.Creator).OrderByDescending(x => x.LikedAt));

            return workouts.Select(x => simpleResponseMapper.Map(x.Workout));
        }

        public async Task<DetailedWorkoutResponseDTO> GetDetailed(string creatorUsername, string workoutName, Guid? userId)
        {
            Guid creatorId = (await userReadSingleSelectedService.Get(x => new { x.Id }, x => x.Username == creatorUsername)
                ?? throw new NotFoundException($"User {creatorUsername} not found")).Id;

            bool validUserId = userId is not null && userId != default;

            var data = await readSingleSelectedService.Get(
                 x => new
                 {
                     likeCount = x.Likes.Count,
                     favoriteCount = x.Favorites.Count,
                     commentCount = x.Comments.Count,
                     isLiked = validUserId && x.Likes.Any(x => x.Id == userId),
                     isFavorited = validUserId && x.Favorites.Any(x => x.Id == userId),
                     workout = x,
                 },
                 x => x.CreatorId == creatorId && x.Name == workoutName,
                 x => x.Include(x => x.Creator)
                       .Include(x => x.Sets)
                       .ThenInclude(x => x.Exercise))
                ?? throw new NotFoundException();

            DetailedWorkoutResponseDTO mapped = detailedResponseMapper.Map(data.workout);
            mapped.FavoriteCount = data.favoriteCount;
            mapped.LikeCount = data.likeCount;
            mapped.CommentCount = data.commentCount;
            mapped.IsLiked = data.isLiked;
            mapped.IsFavorited = data.isFavorited;

            if (!validUserId)
                return mapped;

            IEnumerable<CompletedWorkout> completed = await completedWorkoutReadRangeService.Get(
                criteria: x => x.UserId == userId && x.WorkoutId == mapped.Id,
                limit: 1,
                queryBuilder: x => x.Include(x => x.CompletedSets).OrderByDescending(x => x.CompletedAt));

            if (!completed.Any())
                return mapped;

            CompletedWorkout latest = completed.First();
            mapped.AlreadyAttempted = true;
            mapped.Sets = mapped.Sets.Select(set =>
            {
                CompletedSet? completedSet = latest.CompletedSets.FirstOrDefault(x => x.SetId == set.Id);
                if (completedSet is null)
                    return set;

                set.WeightUsedLastTime = completedSet.WeightUsed;
                set.RepsCompletedLastTime = completedSet.RepsCompleted;
                return set;
            });

            return mapped;
        }
    }
}
