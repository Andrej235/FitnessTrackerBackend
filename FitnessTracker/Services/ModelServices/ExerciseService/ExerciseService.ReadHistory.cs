using FitnessTracker.DTOs.Responses.CompletedWorkouts;
using FitnessTracker.DTOs.Responses.Exercises;
using FitnessTracker.Services.Read;

namespace FitnessTracker.Services.ModelServices.ExerciseService
{
    public partial class ExerciseService
    {
        public async Task<IEnumerable<ExerciseHistorySegmentResponseDTO>> GetHistory(int exerciseId, Guid userId, int? offset, int? limit)
        {
            var recentCompletedWorkouts = await completedWorkoutReadRangeSelectedService.Get(x =>
                new
                {
                    x.CompletedAt,
                    Sets = x.CompletedSets.Select(x => new
                    {
                        x.WeightUsed,
                        x.RepsCompleted
                    }),
                },
                x => x.UserId == userId && x.CompletedSets.Any(cs => cs.Set.ExerciseId == exerciseId),
                offset,
                limit,
                x => x.OrderByDescending(x => x.CompletedAt));

            return recentCompletedWorkouts.Select(x => new ExerciseHistorySegmentResponseDTO()
            {
                CompletionDate = x.CompletedAt,
                SetsCompleted = x.Sets.Select(s => new CompletedSetResponseDTO()
                {
                    Reps = s.RepsCompleted,
                    Weight = s.WeightUsed
                })
            });
        }
    }
}
