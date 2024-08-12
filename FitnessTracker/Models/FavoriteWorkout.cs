namespace FitnessTracker.Models
{
    public class FavoriteWorkout
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public Guid WorkoutId { get; set; }
        public Workout Workout { get; set; } = null!;

        public DateTime FavoritedAt { get; set; }
    }
}
