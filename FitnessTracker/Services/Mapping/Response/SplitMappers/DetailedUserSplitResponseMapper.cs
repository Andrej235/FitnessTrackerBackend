using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.SplitMappers
{
    public class DetailedUserSplitResponseMapper(IResponseMapper<SplitWorkout, SimpleSplitWorkoutResponseDTO> splitWorkoutResponseMapper, IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper) : IResponseMapper<Split, DetailedUserSplitResponseDTO>
    {
        private readonly IResponseMapper<SplitWorkout, SimpleSplitWorkoutResponseDTO> splitWorkoutResponseMapper = splitWorkoutResponseMapper;
        private readonly IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper = userResponseMapper;

        public DetailedUserSplitResponseDTO Map(Split from) => new()
        {
            Id = from.Id,
            Name = from.Name,
            Workouts = from.Workouts.Select(splitWorkoutResponseMapper.Map),
            Creator = userResponseMapper.Map(from.Creator),
        };
    }
}
