namespace FitnessTracker.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public Guid CreatorId { get; set; }
        public User User { get; set; } = null!;
    }
}
