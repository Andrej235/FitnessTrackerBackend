using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.UserMappers
{
    public class SimpleWorkoutPinResponseMapper : IResponseMapper<Workout, SimplePinResponseDTO>
    {
        public SimplePinResponseDTO Map(Workout from) => new()
        {
            Id = from.Id,
            Name = from.Name,
            Description = from.Description ?? "",
            Type = SimplePinResponseDTO.PinType.Workout,
        };
    }
}
