using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Services.ModelServices.SplitService
{
    public partial class SplitService
    {
        public async Task<IEnumerable<SimpleSplitResponseDTO>> GetAllPublic(string? name)
        {
            IEnumerable<Split> splits = string.IsNullOrWhiteSpace(name)
                ? await readRangeService.Get(x => x.IsPublic, 0, 10, x => x.Include(x => x.Creator))
                : await readRangeService.Get(x => x.IsPublic && EF.Functions.Like(x.Name, $"%{name}%"), 0, 10, x => x.Include(x => x.Creator));

            return splits.Select(simpleResponseMapper.Map);
        }

        public async Task<IEnumerable<SimpleSplitResponseDTO>> GetAllPublicBy(string username, string? splitNameFilter)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new InvalidArgumentException($"{nameof(username)} cannot be empty");

            var user = await userReadSingleService.Get(
                x => new
                {
                    x.Id,
                },
                x => x.Username == username)
                ?? throw new NotFoundException();

            IEnumerable<Split> splits = string.IsNullOrWhiteSpace(splitNameFilter)
                ? await readRangeService.Get(x => x.CreatorId == user.Id && x.IsPublic, 0, 10, x => x.Include(x => x.Creator))
                : await readRangeService.Get(x => x.CreatorId == user.Id && x.IsPublic && EF.Functions.Like(x.Name, $"%{splitNameFilter}%"), 0, 10, x => x.Include(x => x.Creator));

            return splits.Select(simpleResponseMapper.Map);
        }

        public async Task<IEnumerable<SimpleSplitResponseDTO>> GetAllSimplePersonal(Guid userId, string? name)
        {
            IEnumerable<Split> splits = name is null
                ? await readRangeService.Get(x => x.CreatorId == userId, 0, 10, x => x.Include(x => x.Creator))
                : await readRangeService.Get(x => x.CreatorId == userId && EF.Functions.Like(x.Name, $"%{name}%"), 0, 10, x => x.Include(x => x.Creator));

            return splits.Select(simpleResponseMapper.Map);
        }

        public async Task<DetailedSplitResponseDTO> GetSingleDetailed(Guid splitId, Guid? userId)
        {
            var data = await readSingleSelectedService.Get(
                x => new
                {
                    likeCount = x.Likes.Count,
                    favoriteCount = x.Favorites.Count,
                    commentCount = x.Comments.Count,
                    isLiked = x.Likes.Any(x => x.Id == userId),
                    isFavorite = x.Favorites.Any(x => x.Id == userId),
                    split = x,
                },
                x => x.Id == splitId,
                x => x.Include(x => x.Creator)
                      .Include(x => x.Workouts)
                      .ThenInclude(x => x.Workout)
                      .ThenInclude(x => x.Creator))
                ?? throw new NotFoundException();

            if (!data.split.IsPublic && data.split.CreatorId != userId)
                throw new UnauthorizedAccessException();

            DetailedSplitResponseDTO mapped = detailedResponseMapper.Map(data.split);
            mapped.LikeCount = data.likeCount;
            mapped.FavoriteCount = data.favoriteCount;
            mapped.CommentCount = data.commentCount;
            mapped.IsLiked = data.isLiked;
            mapped.IsFavorited = data.isFavorite;
            return mapped;
        }

        public async Task<DetailedUserSplitResponseDTO> GetDetailedUsedBy(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new InvalidArgumentException($"{nameof(username)} cannot be empty");

            var user = await userReadSingleService.Get(
                x => new
                {
                    x.Id,
                    x.SplitId,
                    x.Settings.PublicStreak
                },
                x => x.Username == username,
                x => x.Include(x => x.Settings))
                ?? throw new NotFoundException();

            if (!user.PublicStreak)
                throw new UnauthorizedAccessException();

            Split? split = await readSingleService.Get(
                x => x.Id == user.SplitId,
                x => x.Include(x => x.Creator)
                      .Include(x => x.Workouts)
                      .ThenInclude(x => x.Workout))
                ?? throw new NotFoundException();

            if (!split.IsPublic)
                throw new UnauthorizedAccessException();

            DetailedUserSplitResponseDTO mapped = detailedUserSplitResponseMapper.Map(split);
            return mapped;
        }
    }
}
