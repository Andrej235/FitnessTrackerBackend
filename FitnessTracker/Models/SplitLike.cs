namespace FitnessTracker.Models
{
    public class SplitLike
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public Guid SplitId { get; set; }
        public Split Split { get; set; } = null!;

        public DateTime LikedAt { get; set; }
    }
}
