﻿using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Services.ModelServices.SplitService
{
    public partial class SplitService
    {
        public async Task<IEnumerable<SimpleSplitResponseDTO>> GetAllPublicBy(string username, Guid? userId, string? splitNameFilter, int? offset, int? limit)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new InvalidArgumentException($"{nameof(username)} cannot be empty");

            var user = await userReadSingleSelectedService.Get(
                x => new
                {
                    x.Id,
                    x.Settings.PublicCreatedSplits,
                },
                x => x.Username == username)
                ?? throw new NotFoundException($"User {username} not found.");

            if (!user.PublicCreatedSplits && userId != user.Id)
                throw new AccessDeniedException("User's created splits are private.");

            IEnumerable<Split> splits = string.IsNullOrWhiteSpace(splitNameFilter)
                ? await readRangeService.Get(x => x.CreatorId == user.Id, offset, limit, x => x.Include(x => x.Creator).OrderByDescending(x => x.CreatedAt))
                : await readRangeService.Get(x => x.CreatorId == user.Id && EF.Functions.Like(x.Name, $"%{splitNameFilter}%"), offset, limit, x => x.Include(x => x.Creator).OrderByDescending(x => x.CreatedAt));

            return splits.Select(simpleResponseMapper.Map);
        }

        public async Task<IEnumerable<SimpleSplitResponseDTO>> GetAllFavoritesBy(string username, Guid? userId, string? nameFilter, int? limit, int? offset)
        {
            var user = await userReadSingleSelectedService.Get(
                x => new
                {
                    x.Id,
                    x.Settings.PublicFavoriteSplits,
                },
                x => x.Username == username)
                ?? throw new NotFoundException($"User {username} not found.");

            if (!user.PublicFavoriteSplits && userId != user.Id)
                throw new AccessDeniedException("User's favorite splits are private.");

            IEnumerable<FavoriteSplit> splits = nameFilter is null
                ? await favoriteReadRangeService.Get(x => x.UserId == user.Id, offset, limit ?? 10, x => x.Include(x => x.Split).ThenInclude(x => x.Creator).OrderByDescending(x => x.FavoritedAt))
                : await favoriteReadRangeService.Get(x => x.UserId == user.Id && EF.Functions.Like(x.Split.Name, $"%{nameFilter}%"), offset, limit ?? 10, x => x.Include(x => x.Split).ThenInclude(x => x.Creator).OrderByDescending(x => x.FavoritedAt));

            return splits.Select(x => simpleResponseMapper.Map(x.Split));
        }

        public async Task<IEnumerable<SimpleSplitResponseDTO>> GetAllLikedBy(string username, Guid? userId, string? nameFilter, int? limit, int? offset)
        {
            var user = await userReadSingleSelectedService.Get(
                x => new
                {
                    x.Id,
                    x.Settings.PublicLikedSplits,
                },
                x => x.Username == username)
                ?? throw new NotFoundException($"User {username} not found.");

            if (!user.PublicLikedSplits && userId != user.Id)
                throw new AccessDeniedException("User's liked splits are private.");

            IEnumerable<SplitLike> splits = nameFilter is null
                ? await likeReadRangeService.Get(x => x.UserId == user.Id, offset, limit ?? 10, x => x.Include(x => x.Split).ThenInclude(x => x.Creator).OrderByDescending(x => x.LikedAt))
                : await likeReadRangeService.Get(x => x.UserId == user.Id && EF.Functions.Like(x.Split.Name, $"%{nameFilter}%"), offset, limit ?? 10, x => x.Include(x => x.Split).ThenInclude(x => x.Creator).OrderByDescending(x => x.LikedAt));

            return splits.Select(x => simpleResponseMapper.Map(x.Split));
        }

        public async Task<DetailedSplitResponseDTO> GetSingleDetailed(string creatorUsername, string splitName, Guid? userId)
        {
            Guid creatorId = (await userReadSingleSelectedService.Get(x => new { x.Id }, x => x.Username == creatorUsername)
                ?? throw new NotFoundException($"User {creatorUsername} not found")).Id;

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
                x => x.CreatorId == creatorId && x.Name == splitName,
                x => x.Include(x => x.Creator)
                      .Include(x => x.Workouts)
                      .ThenInclude(x => x.Workout)
                      .ThenInclude(x => x.Creator))
                ?? throw new NotFoundException();

            DetailedSplitResponseDTO mapped = detailedResponseMapper.Map(data.split);
            mapped.LikeCount = data.likeCount;
            mapped.FavoriteCount = data.favoriteCount;
            mapped.CommentCount = data.commentCount;
            mapped.IsLiked = data.isLiked;
            mapped.IsFavorited = data.isFavorite;
            return mapped;
        }

        public async Task<DetailedUserSplitResponseDTO> GetDetailedUsedBy(string username, Guid? userId)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new InvalidArgumentException($"{nameof(username)} cannot be empty");

            var user = await userReadSingleSelectedService.Get(
                x => new
                {
                    x.Id,
                    x.SplitId,
                    x.Settings.PublicCurrentSplit
                },
                x => x.Username == username)
                ?? throw new NotFoundException();

            if (!user.PublicCurrentSplit && userId != user.Id)
                throw new AccessDeniedException();

            Split? split = await readSingleService.Get(
                x => x.Id == user.SplitId,
                x => x.Include(x => x.Creator)
                      .Include(x => x.Workouts)
                      .ThenInclude(x => x.Workout))
                ?? throw new NotFoundException();

            DetailedUserSplitResponseDTO mapped = detailedUserSplitResponseMapper.Map(split);
            return mapped;
        }
    }
}
