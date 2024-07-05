namespace FitnessTracker.Models
{
    public class CompletedWorkout
    {
        public Guid Id { get; set; }
        public DateTime CompletedAt { get; set; }

        public Guid UserId { get; set; }

        public Guid? WorkoutId { get; set; }
        public Workout? Workout { get; set; }

        public Guid SplitId { get; set; }
        public Split Split { get; set; } = null!;

        public ICollection<CompletedSet> CompletedSets { get; set; } = [];
    }
}
