using FitnessTracker.DTOs.Responses.Exercises;
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
            {
                mapped.IsFavorite = await favoriteReadSingleService.Get(x => x.UserId == userId && x.ExerciseId == exerciseId) is not null;

                var mostWeightLifted = (await completedSetReadRangeSelectedService.Get(
                    x => new
                    {
                        x.WeightUsed,
                        x.RepsCompleted,
                        x.CompletedWorkout.CompletedAt
                    },
                    x => x.UserId == userId && x.Set.ExerciseId == exerciseId,
                    0,
                    1,
                    x => x.OrderByDescending(x => x.WeightUsed))).FirstOrDefault();

                if (mostWeightLifted is null)
                {
                    mapped.MostWeightLifted = null;
                    mapped.MostVolumeLifted = null;
                    return mapped;
                }

                var mostVolumeLifted = (await completedSetReadRangeSelectedService.Get(
                    x => new
                    {
                        x.WeightUsed,
                        x.RepsCompleted,
                        x.CompletedWorkout.CompletedAt
                    },
                    x => x.UserId == userId && x.Set.ExerciseId == exerciseId,
                    0,
                    1,
                    x => x.OrderByDescending(x => x.WeightUsed * x.RepsCompleted))).First();

                var mostSessionVolumeLifter = (await completedWorkoutReadRangeSelectedService.Get(
                    x => new
                    {
                        TotalVolume = x.CompletedSets.Where(x => x.Set.ExerciseId == exerciseId).Sum(cs => cs.WeightUsed * cs.RepsCompleted),
                        x.CompletedAt,
                    },
                    x => x.UserId == userId && x.CompletedSets.Any(cs => cs.Set.ExerciseId == exerciseId),
                    0,
                    1,
                    x => x.OrderByDescending(x => x.CompletedSets.Where(x => x.Set.ExerciseId == exerciseId).Sum(cs => cs.WeightUsed * cs.RepsCompleted))
                    )).First();

                mapped.MostWeightLifted = new()
                {
                    AchievedAt = mostWeightLifted.CompletedAt,
                    Reps = mostWeightLifted.RepsCompleted,
                    Weight = mostWeightLifted.WeightUsed
                };

                mapped.MostVolumeLifted = new()
                {
                    AchievedAt = mostVolumeLifted.CompletedAt,
                    Reps = mostVolumeLifted.RepsCompleted,
                    Weight = mostVolumeLifted.WeightUsed
                };

                mapped.MostSessionVolumeLifted = new()
                {
                    AchievedAt = mostVolumeLifted.CompletedAt,
                    TotalVolumeLifted = mostSessionVolumeLifter.TotalVolume
                };
            }

            return mapped;
        }
    }
}
