namespace FitnessTracker.Models
{
    public class SplitWorkout
    {
        public enum DayOfWeek
        {
            Monday = 0,
            Tuesday = 1,
            Wednesday = 2,
            Thursday = 3,
            Friday = 4,
            Saturday = 5,
            Sunday = 6,
        }

        public Workout Workout { get; set; } = null!;
        public Guid WorkoutId { get; set; }
        public Split Split { get; set; } = null!;
        public Guid SplitId { get; set; }
        public DayOfWeek Day { get; set; } = DayOfWeek.Monday;
    }
}
