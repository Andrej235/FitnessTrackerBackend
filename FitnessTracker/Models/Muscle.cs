namespace FitnessTracker.Models
{
    public class Muscle
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public IEnumerable<Exercise> PrimaryInExercises { get; set; } = new HashSet<Exercise>();
        public IEnumerable<Exercise> SecondaryInExercises { get; set; } = new HashSet<Exercise>();

        public MuscleGroup MuscleGroup { get; set; } = null!;
        public int MuscleGroupId { get; set; }
    }
}