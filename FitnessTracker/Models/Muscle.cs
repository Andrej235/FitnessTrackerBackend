namespace FitnessTracker.Models
{
    public class Muscle
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<Exercise> PrimaryInExercises { get; set; } = [];
        public ICollection<Exercise> SecondaryInExercises { get; set; } = [];

        public MuscleGroup MuscleGroup { get; set; } = null!;
        public int MuscleGroupId { get; set; }
    }
}