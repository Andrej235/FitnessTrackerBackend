using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Exceptions;
using FitnessTracker.Services.Read;

namespace FitnessTracker.Services.ModelServices.WorkoutService
{
    public partial class WorkoutService
    {
        public async Task<WorkoutExerciseChartDataResponseDTO> GetChartDataForExercise(Guid userId, string username, string workoutName, int exerciseId, DateTime? startDate)
        {
            Guid workoutId = (await readSingleSelectedService.Get(
                x => new { x.Id },
                x => x.Creator.Username == username && x.Name == workoutName)
                ?? throw new NotFoundException($"Workout {workoutName} not found")).Id;

            startDate ??= DateTime.Now.AddMonths(-3);
            var completedWorkouts = await completedWorkoutReadRangeSelectedService.Get(
                x => new
                {
                    Sets = x.CompletedSets.Select(y => new
                    {
                        y.WeightUsed,
                        y.RepsCompleted,
                        y.Set.Type
                    }),
                    x.CompletedAt
                },
                x => x.CompletedAt >= startDate && x.WorkoutId == workoutId && x.UserId == userId && x.CompletedSets.Any(x => x.Set.ExerciseId == exerciseId),
                queryBuilder: x => x.OrderByDescending(x => x.CompletedAt));

            var sets = completedWorkouts
                .Select(x => new
                {
                    x.CompletedAt,
                    Set = x.Sets.Where(x => x.Type is Models.Set.SetType.Normal or Models.Set.SetType.Failure).MinBy(x => x.WeightUsed)
                });

            return new WorkoutExerciseChartDataResponseDTO()
            {
                Data = sets.Where(x => x.Set is not null).Select(x => new WorkoutExerciseChartSingleSetDataResponseDTO()
                {
                    TimeCompleted = x.CompletedAt,
                    RepsCompleted = x.Set!.RepsCompleted,
                    WeightUsed = x.Set!.WeightUsed,
                })
            };
        }
    }
}
