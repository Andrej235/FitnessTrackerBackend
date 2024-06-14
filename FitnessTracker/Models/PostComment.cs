namespace FitnessTracker.Models
{
    public class PostComment
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid CreatorId { get; set; }
        public User Creator { get; set; } = null!;

        public string Text { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public Guid? ParentId { get; set; }
        public IEnumerable<PostComment> Children { get; set; } = new HashSet<PostComment>();
        public IEnumerable<User> Likes { get; set; } = new HashSet<User>();
    }
}
