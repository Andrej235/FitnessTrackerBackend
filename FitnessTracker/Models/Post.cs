namespace FitnessTracker.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;

        public int CreatorId { get; set; }
        public User Creator { get; set; } = null!;

        public IEnumerable<PostLike> Likes { get; set; } = new HashSet<PostLike>();
        public IEnumerable<PostComment> Comments { get; set; } = new HashSet<PostComment>();
    }
}
