namespace FitnessTracker.Models
{
    public class WorkoutPin
    {
        public Guid WorkoutId { get; set; }
        public Workout Workout { get; set; } = null!;

        public Guid UserId { get; set; }
    }
}
