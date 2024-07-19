using FitnessTracker.DTOs.Responses.CompletedWorkouts;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.CompletedWorkoutMappers
{
    public class SimpleWeekOfCompletedWorkoutsResponseMapper : IResponseMapper<IGrouping<DateTime, CompletedWorkout>, SimpleWeekOfCompletedWorkoutsResponseDTO>
    {
        public SimpleWeekOfCompletedWorkoutsResponseDTO Map(IGrouping<DateTime, CompletedWorkout> from) => new()
        {
            StartDate = from.Key,
            CompletedCount = from.Count(),
            TotalCount = from.First().Split.Workouts.Count
        };
    }
}
