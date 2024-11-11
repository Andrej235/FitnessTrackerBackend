using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Exceptions;
using FitnessTracker.Services.Read;

namespace FitnessTracker.Services.ModelServices.WorkoutService
{
    public partial class WorkoutService
    {
        public async Task<WorkoutExerciseChartDataResponseDTO> GetChartDataForExercise(Guid userId, string username, string workoutName, int exerciseId, int? limit, int? offset)
        {
            Guid workoutId = (await readSingleSelectedService.Get(
                x => new { x.Id },
                x => x.Creator.Username == username && x.Name == workoutName)
                ?? throw new NotFoundException($"Workout {workoutName} not found")).Id;

            var data = await completedWorkoutReadRangeSelectedService.Get(
                x => new
                {
                    Sets = x.CompletedSets.Select(x => new
                    {
                        x.WeightUsed,
                        x.RepsCompleted
                    }),
                    x.CompletedAt
                },
                x => x.WorkoutId == workoutId && x.CompletedSets.Any(x => x.Set.ExerciseId == exerciseId) && x.UserId == userId,
                offset: offset ?? 0,
                limit: limit ?? 10,
                queryBuilder: x => x.OrderByDescending(x => x.CompletedAt));

            return new WorkoutExerciseChartDataResponseDTO()
            {
                Data = data.SelectMany(x => x.Sets.Select(y => new WorkoutExerciseChartSingleSetDataResponseDTO()
                {
                    TimeCompleted = x.CompletedAt,
                    RepsCompleted = y.RepsCompleted,
                    WeightUsed = y.WeightUsed
                }))
            };
        }
    }
}
