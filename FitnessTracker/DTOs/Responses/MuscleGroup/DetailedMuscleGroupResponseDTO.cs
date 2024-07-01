using FitnessTracker.DTOs.Responses.Muscle;

namespace FitnessTracker.DTOs.Responses.MuscleGroup
{
    public class DetailedMuscleGroupResponseDTO : SimpleMuscleGroupResponseDTO
    {
        public IEnumerable<SimpleMuscleResponseDTO> Muscles { get; set; } = [];
    }
}
