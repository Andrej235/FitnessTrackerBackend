
namespace FitnessTracker.DTOs
{
    public class MuscleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int MuscleGroupId { get; set; }
        public IEnumerable<int> PrimaryInExercises { get; set; } = [];
        public IEnumerable<int> SecondaryInExercises { get; set; } = [];
    }
}
