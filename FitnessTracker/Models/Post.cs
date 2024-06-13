namespace FitnessTracker.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int CreatorId { get; set; }
        public User Creator { get; set; }
    }
}
