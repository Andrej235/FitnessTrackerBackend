namespace FitnessTracker.Models
{
    public class UserFollows
    {
        public Guid FollowerId { get; set; }
        public Guid FolloweeId { get; set; }
    }
}
