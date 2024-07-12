using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.WorkoutMappers
{
    public class NewWorkoutResponseMapper : IResponseMapper<Workout, NewWorkoutResponseDTO>
    {
        public NewWorkoutResponseDTO Map(Workout from)
        {
            return new()
            {
                Id = from.Id,
                Name = from.Name,
                IsPublic = from.IsPublic,
            };
        }
    }
}
