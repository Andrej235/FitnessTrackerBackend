namespace FitnessTracker.Models
{
    public class CompletedWorkout
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? WorkoutId { get; set; }
        public Workout? Workout { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
