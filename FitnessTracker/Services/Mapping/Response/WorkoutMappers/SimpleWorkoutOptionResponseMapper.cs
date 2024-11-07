using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.WorkoutMappers
{
    public class SimpleWorkoutOptionResponseMapper(IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper) : IResponseMapper<Workout, SimpleWorkoutOptionResponseDTO>
    {
        private readonly IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper = userResponseMapper;

        public SimpleWorkoutOptionResponseDTO Map(Workout from) => new()
        {
            Id = from.Id,
            Description = from.Description ?? "",
            Name = from.Name,
            Creator = userResponseMapper.Map(from.Creator)
        };
    }
}
