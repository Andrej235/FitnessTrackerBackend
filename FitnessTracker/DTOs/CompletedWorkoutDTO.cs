namespace FitnessTracker.DTOs
{
    public class CompletedWorkoutDTO
    {
        public WorkoutDTO? Workout { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
