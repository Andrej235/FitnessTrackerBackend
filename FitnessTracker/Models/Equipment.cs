namespace FitnessTracker.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<Exercise> UsedInExercises { get; set; } = [];
    }
}