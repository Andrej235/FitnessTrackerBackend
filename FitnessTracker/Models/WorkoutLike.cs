namespace FitnessTracker.Models
{
    public class WorkoutLike
    {
        public Guid WorkoutId { get; set; }
        public Workout Workout { get; set; } = null!;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public DateTime LikedAt { get; set; }
    }
}
