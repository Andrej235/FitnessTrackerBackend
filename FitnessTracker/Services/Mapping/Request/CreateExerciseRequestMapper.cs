using FitnessTracker.DTOs.Requests.Exercise;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Request
{
    public class CreateExerciseRequestMapper : IRequestMapper<CreateExerciseRequestDTO, Exercise>
    {
        public Exercise Map(CreateExerciseRequestDTO from)
        {
            return new Exercise
            {
                Name = from.Name,
                Description = from.Description,
                Image = from.Image ?? "",
            };
        }
    }
}
