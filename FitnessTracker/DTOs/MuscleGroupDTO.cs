namespace FitnessTracker.DTOs
{
    public class MuscleGroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public IEnumerable<int> MuscleIds { get; set; } = [];
        public IEnumerable<int> PrimaryInExercises { get; set; } = [];
        public IEnumerable<int> SecondaryInExercises { get; set; } = [];
    }
}
