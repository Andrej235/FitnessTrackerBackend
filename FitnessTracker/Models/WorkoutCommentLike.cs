namespace FitnessTracker.Models
{
    public class WorkoutCommentLike
    {
        public Guid UserId { get; set; }
        public Guid WorkoutCommentId { get; set; }
    }
}
