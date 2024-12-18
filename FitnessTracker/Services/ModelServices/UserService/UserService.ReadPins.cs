﻿using FitnessTracker.DTOs.Responses.Pins;
using FitnessTracker.Exceptions;
using FitnessTracker.Services.Read;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public partial class UserService
    {
        public async Task<IEnumerable<PinResponseDTO>> GetPinsFor(string username)
        {
            var pins = await readSingleSelectedService.Get(
                x => new
                {
                    WorkoutPins = x.WorkoutPins.Select(x => new
                    {
                        Pin = x,
                        LikeCount = x.Workout.Likes.Count,
                        FavoriteCount = x.Workout.Favorites.Count,
                    }),
                    SplitPins = x.SplitPins.Select(x => new
                    {
                        Pin = x,
                        LikeCount = x.Split.Likes.Count,
                        FavoriteCount = x.Split.Favorites.Count,
                    }),
                },
                x => x.Username == username,
                x => x.Include(x => x.WorkoutPins)
                    .ThenInclude(x => x.Workout)
                    .Include(x => x.SplitPins)
                    .ThenInclude(x => x.Split))
                ?? throw new NotFoundException($"User '{username}' not found");

            IEnumerable<PinResponseDTO> mappedWorkoutPins = pins.WorkoutPins.Select(x =>
            {
                PinResponseDTO mapped = workoutPinResponseMapper.Map(x.Pin);
                mapped.LikeCount = x.LikeCount;
                mapped.FavoriteCount = x.FavoriteCount;
                return mapped;
            });

            IEnumerable<PinResponseDTO> mappedSplitPins = pins.SplitPins.Select(x =>
            {
                PinResponseDTO mapped = splitPinResponseMapper.Map(x.Pin);
                mapped.LikeCount = x.LikeCount;
                mapped.FavoriteCount = x.FavoriteCount;
                return mapped;
            });

            return mappedWorkoutPins.Union(mappedSplitPins).OrderBy(x => x.Order);
        }
    }
}
