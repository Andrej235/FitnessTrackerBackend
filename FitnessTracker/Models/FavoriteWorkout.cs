namespace FitnessTracker.Models
{
    public class FavoriteWorkout
    {
        public Guid UserId { get; set; }
        public Guid WorkoutId { get; set; }
    }
}
