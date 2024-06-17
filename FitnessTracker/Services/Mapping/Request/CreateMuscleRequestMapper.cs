using FitnessTracker.DTOs.Requests.Muscle;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Request
{
    public class CreateMuscleRequestMapper : IRequestMapper<CreateMuscleRequestDTO, Muscle>
    {
        public Muscle Map(CreateMuscleRequestDTO from)
        {
            return new()
            {
                Name = from.Name,
                MuscleGroupId = from.MuscleGroupId,
            };
        }
    }
}
