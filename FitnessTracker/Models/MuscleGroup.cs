namespace FitnessTracker.Models
{
    public class MuscleGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<Exercise> PrimaryInExercises { get; set; } = [];
        public ICollection<Exercise> SecondaryInExercises { get; set; } = [];
    }
}
