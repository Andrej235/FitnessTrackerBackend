namespace FitnessTracker.Models
{
    public class SplitComment
    {
        public Guid Id { get; set; }
        public Guid SplitId { get; set; }
        public Guid CreatorId { get; set; }
        public User Creator { get; set; } = null!;

        public string Text { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public Guid? ParentId { get; set; }
        public IEnumerable<SplitComment> Children { get; set; } = new HashSet<SplitComment>();
        public IEnumerable<User> Likes { get; set; } = new HashSet<User>();
    }
}
