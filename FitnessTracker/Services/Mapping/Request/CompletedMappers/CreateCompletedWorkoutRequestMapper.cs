using FitnessTracker.DTOs.Requests.Completed;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Request.CompletedMappers
{
    public class CreateCompletedWorkoutRequestMapper(IRequestMapper<CreateCompletedSetRequestDTO, CompletedSet> setRequestMapper) : IRequestMapper<CreateCompletedWorkoutRequestDTO, CompletedWorkout>
    {
        public CompletedWorkout Map(CreateCompletedWorkoutRequestDTO from)
        {
            return new()
            {
                CompletedSets = from.CompletedSets.Select(setRequestMapper.Map).ToList(),
                CompletedAt = DateTime.UtcNow
            };
        }
    }
}
