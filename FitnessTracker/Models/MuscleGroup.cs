namespace FitnessTracker.Models
{
    public class MuscleGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public IEnumerable<Exercise> PrimaryInExercises { get; set; } = new HashSet<Exercise>();
        public IEnumerable<Exercise> SecondaryInExercises { get; set; } = new HashSet<Exercise>();
    }
}
