using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.SplitMappers
{
    public class SimpleSplitWorkoutResponseMapper(IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper) : IResponseMapper<SplitWorkout, SimpleSplitWorkoutResponseDTO>
    {
        private readonly IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper = userResponseMapper;

        public SimpleSplitWorkoutResponseDTO Map(SplitWorkout from) => new()
        {
            Creator = userResponseMapper.Map(from.Workout.Creator),
            Id = from.WorkoutId,
            Name = from.Workout.Name,
            Description = from.Workout.Description,
            Day = from.Day,
        };
    }
}
