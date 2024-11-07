using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.WorkoutMappers
{
    public class SimpleWorkoutResponseMapper(IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper) : IResponseMapper<Workout, SimpleWorkoutResponseDTO>
    {
        private readonly IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper = userResponseMapper;

        public SimpleWorkoutResponseDTO Map(Workout from) => new()
        {
            Id = from.Id,
            Name = from.Name,
            Description = from.Description ?? "",
            Creator = userResponseMapper.Map(from.Creator),
        };
    }
}
