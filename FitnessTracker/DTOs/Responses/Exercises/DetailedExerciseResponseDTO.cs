using FitnessTracker.DTOs.Responses.Equipment;
using FitnessTracker.DTOs.Responses.Muscle;
using FitnessTracker.DTOs.Responses.MuscleGroup;

namespace FitnessTracker.DTOs.Responses.Exercises
{
    public class DetailedExerciseResponseDTO : BaseExerciseResponseDTO
    {
        public string Description { get; set; } = null!;
        public IEnumerable<SimpleMuscleGroupResponseDTO> PrimaryMuscleGroups { get; set; } = [];
        public IEnumerable<SimpleMuscleResponseDTO> PrimaryMuscles { get; set; } = [];
        public IEnumerable<SimpleMuscleGroupResponseDTO> SecondaryMuscleGroups { get; set; } = [];
        public IEnumerable<SimpleMuscleResponseDTO> SecondaryMuscles { get; set; } = [];
        public IEnumerable<SimpleEquipmentResponseDTO> Equipment { get; set; } = [];
    }
}
