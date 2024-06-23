namespace FitnessTracker.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;

        public Guid CreatorId { get; set; }
        public User Creator { get; set; } = null!;

        public ICollection<User> Likes { get; set; } = [];
        public ICollection<PostComment> Comments { get; set; } = [];
    }
}
