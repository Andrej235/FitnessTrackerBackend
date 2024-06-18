namespace FitnessTracker.DTOs.Requests.Exercise
{
    public class BaseExerciseRequestDTO
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Image { get; set; }

        public IEnumerable<int> PrimaryMuscleGroups { get; set; } = [];
        public IEnumerable<int> PrimaryMuscles { get; set; } = [];
        public IEnumerable<int> SecondaryMuscleGroups { get; set; } = [];
        public IEnumerable<int> SecondaryMuscles { get; set; } = [];
        public IEnumerable<int> Equipment { get; set; } = [];
    }
}
