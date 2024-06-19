using FitnessTracker.DTOs.Requests.Exercise;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Request.ExerciseMappers
{
    public class UpdateExerciseRequestMapper : IRequestMapper<UpdateExerciseRequestDTO, Exercise>
    {
        public Exercise Map(UpdateExerciseRequestDTO from)
        {
            return new()
            {
                Id = from.Id,
                Name = from.Name,
                Description = from.Description,
                Image = from.Image ?? ""
            };
        }
    }
}
