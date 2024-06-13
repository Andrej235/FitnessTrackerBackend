namespace FitnessTracker.Models
{
    public class WorkoutComment
    {
        public Guid WorkoutId { get; set; }
        public Guid CreatorId { get; set; }
        public User Creator { get; set; } = null!;

        public string Text { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public Guid? ParentId { get; set; }
        public IEnumerable<WorkoutComment> Children { get; set; } = new HashSet<WorkoutComment>();
        public IEnumerable<User> Likes { get; set; } = new HashSet<User>();
    }
}
