using FitnessTracker.DTOs.Enums;
using FitnessTracker.DTOs.Responses.Pins;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.UserMappers
{
    public class SimpleWorkoutPinResponseMapper : IResponseMapper<WorkoutPin, PinResponseDTO>
    {
        public PinResponseDTO Map(WorkoutPin from) => new()
        {
            Id = from.Workout.Id,
            Name = from.Workout.Name,
            Description = from.Workout.Description ?? "",
            Type = PinType.Workout,
            Order = from.Order
        };
    }
}
