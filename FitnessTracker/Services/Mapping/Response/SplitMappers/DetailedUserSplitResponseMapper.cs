using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.SplitMappers
{
    public class DetailedUserSplitResponseMapper(IResponseMapper<SplitWorkout, SimpleSplitWorkoutResponseDTO> splitWorkoutResponseMapper) : IResponseMapper<Split, DetailedUserSplitResponseDTO>
    {
        private readonly IResponseMapper<SplitWorkout, SimpleSplitWorkoutResponseDTO> splitWorkoutResponseMapper = splitWorkoutResponseMapper;

        public DetailedUserSplitResponseDTO Map(Split from) => new()
        {
            Id = from.Id,
            Name = from.Name,
            Workouts = from.Workouts.Select(splitWorkoutResponseMapper.Map),
        };
    }
}
