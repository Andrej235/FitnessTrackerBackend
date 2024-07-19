using FitnessTracker.DTOs.Requests.Split;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Request.SplitMappers
{
    public class CreateSplitRequestMapper(IRequestMapper<CreateSplitWorkoutRequestDTO, SplitWorkout> splitWorkoutRequestMapper) : IRequestMapper<CreateSplitRequestDTO, Split>
    {
        private readonly IRequestMapper<CreateSplitWorkoutRequestDTO, SplitWorkout> splitWorkoutRequestMapper = splitWorkoutRequestMapper;

        public Split Map(CreateSplitRequestDTO from) => new()
        {
            Name = from.Name,
            Description = from.Description,
            IsPublic = from.IsPublic,
            Workouts = from.Workouts.Select(splitWorkoutRequestMapper.Map).ToList()
        };
    }
}
