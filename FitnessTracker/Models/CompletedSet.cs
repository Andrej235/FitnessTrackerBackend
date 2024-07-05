namespace FitnessTracker.Models
{
    public class CompletedSet
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public Guid CompletedWorkoutId { get; set; }
        public CompletedWorkout CompletedWorkout { get; set; } = null!;

        public Set Set { get; set; } = null!;
        public Guid SetId { get; set; }

        public float WeightUsed { get; set; }
        public int RepsCompleted { get; set; }
    }
}
