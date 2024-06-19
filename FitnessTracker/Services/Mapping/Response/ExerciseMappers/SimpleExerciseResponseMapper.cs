using FitnessTracker.DTOs.Responses.Exercises;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.ExerciseMappers
{
    public class SimpleExerciseResponseMapper : IResponseMapper<Exercise, SimpleExerciseResponseDTO>
    {
        public SimpleExerciseResponseDTO Map(Exercise from)
        {
            return new()
            {
                Id = from.Id,
                Name = from.Name,
                Image = from.Image
            };
        }
    }
}
