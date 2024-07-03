using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.WorkoutMappers
{
    public class SimpleWorkoutResponseMapper(IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper) : IResponseMapper<Workout, SimpleWorkoutResponseDTO>
    {
        private readonly IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper = userResponseMapper;

        public SimpleWorkoutResponseDTO Map(Workout from)
        {
            return new()
            {
                Id = from.Id,
                Name = from.Name,
                IsPublic = from.IsPublic,
                Creator = userResponseMapper.Map(from.Creator),
            };
        }
    }
}
