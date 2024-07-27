using FitnessTracker.DTOs.Responses.CompletedWorkouts;
using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.CompletedWorkoutMappers
{
    public class DetailedWeekOfCompletedWorkoutsResponseMapper(IResponseMapper<Split, DetailedUserSplitResponseDTO> splitResponseMapper) : IResponseMapper<IEnumerable<CompletedWorkout>, DetailedWeekOfCompletedWorkoutsResponseDTO>
    {
        private readonly IResponseMapper<Split, DetailedUserSplitResponseDTO> splitResponseMapper = splitResponseMapper;

        public DetailedWeekOfCompletedWorkoutsResponseDTO Map(IEnumerable<CompletedWorkout> from) => new()
        {
            Split = from.Any() ? splitResponseMapper.Map(from.First().Split) : null,
            CompletedWorkouts = from.Select(x => x.CompletedAt.DayOfWeek)
        };
    }
}
