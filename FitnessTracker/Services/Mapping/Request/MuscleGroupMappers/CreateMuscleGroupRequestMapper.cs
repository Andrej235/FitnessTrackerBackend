using FitnessTracker.DTOs.Requests.MuscleGroup;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Request.MuscleGroupMappers
{
    public class CreateMuscleGroupRequestMapper : IRequestMapper<CreateMuscleGroupRequestDTO, MuscleGroup>
    {
        public MuscleGroup Map(CreateMuscleGroupRequestDTO from)
        {
            return new MuscleGroup()
            {
                Name = from.Name,
            };
        }
    }
}
