using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.UserMappers
{
    public class SimpleWorkoutPinResponseMapper(IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper) : IResponseMapper<Workout, SimplePinResponseDTO>
    {
        private readonly IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper = userResponseMapper;

        public SimplePinResponseDTO Map(Workout from) => new()
        {
            Id = from.Id,
            Name = from.Name,
            Description = from.Description ?? "",
            Creator = userResponseMapper.Map(from.Creator),
            Type = SimplePinResponseDTO.PinType.Workout,
        };
    }
}
