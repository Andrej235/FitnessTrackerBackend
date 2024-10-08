﻿using FitnessTracker.DTOs.Responses.Exercises;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;
using FitnessTracker.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FitnessTracker.Services.ModelServices.ExerciseService
{
    public partial class ExerciseService
    {
        public async Task<IEnumerable<SimpleExerciseResponseDTO>> GetAll(int? muscleGroupId, int? equipmentId, string? name, int? offset, int? limit)
        {
            List<Expression<Func<Exercise, bool>>> filters = [];

            if (name is not null)
                filters.Add(e => EF.Functions.Like(e.Name, $"%{name}%"));

            if (muscleGroupId is not null)
                filters.Add(e => e.PrimaryMuscleGroups.Any(m => m.Id == muscleGroupId) || e.SecondaryMuscleGroups.Any(m => m.Id == muscleGroupId));

            if (equipmentId is not null)
                filters.Add(e => e.Equipment.Any(eq => eq.Id == equipmentId));

            IEnumerable<Exercise> exercises = await readRangeService.Get(
                filters.Combine() ?? (null),
                offset,
                limit ?? 10,
                x =>
                {
                    if (muscleGroupId is not null)
                        x = x.Include(x => x.PrimaryMuscleGroups).Include(x => x.SecondaryMuscleGroups);

                    if (equipmentId is not null)
                        x = x.Include(x => x.Equipment);

                    return x;
                });

            //TODO: Sort the results based on how many criteria they match
            return exercises.Select(simpleResponseMapper.Map);
        }

        public async Task<DetailedExerciseResponseDTO> GetDetailed(int exerciseId, Guid? userId)
        {
            var exercise = await readSingleSelectedService.Get(
                x => new
                {
                    Exercise = x,
                    Favorites = x.Favorites.Count,
                },
                x => x.Id == exerciseId,
                x => x.Include(x => x.PrimaryMuscleGroups)
                      .Include(x => x.SecondaryMuscleGroups)
                      .Include(x => x.PrimaryMuscles)
                      .Include(x => x.SecondaryMuscles)
                      .Include(x => x.Equipment)
                ) ?? throw new NotFoundException($"Exercise with id {exerciseId} was not found");

            DetailedExerciseResponseDTO mapped = detailedResponseMapper.Map(exercise.Exercise);
            mapped.Favorites = exercise.Favorites;

            if (userId is not null)
                mapped.IsFavorite = await favoriteReadSingleService.Get(x => x.UserId == userId && x.ExerciseId == exerciseId) is not null;

            return mapped;
        }
    }
}
