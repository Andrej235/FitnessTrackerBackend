namespace FitnessTracker.Models
{
    public class UserFollows
    {
        public Guid FollowerId { get; set; }
        public User Follower { get; set; } = null!;
        public Guid FolloweeId { get; set; }
        public User Followee { get; set; } = null!;
    }
}
