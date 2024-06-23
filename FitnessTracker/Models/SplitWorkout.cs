namespace FitnessTracker.Models
{
    public class SplitWorkout
    {
        public Workout Workout { get; set; } = null!;
        public Guid WorkoutId { get; set; }
        public Split Split { get; set; } = null!;
        public Guid SplitId { get; set; }
        public DayOfWeek Day { get; set; } = DayOfWeek.Monday;
    }
}
