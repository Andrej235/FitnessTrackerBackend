namespace FitnessTracker.Models
{
    public class WorkoutComment
    {
        public Guid Id { get; set; }
        public Guid WorkoutId { get; set; }
        public Guid CreatorId { get; set; }
        public User Creator { get; set; } = null!;

        public string Text { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public Guid? ParentId { get; set; }
        public ICollection<WorkoutComment> Children { get; set; } = [];
        public ICollection<User> Likes { get; set; } = [];
    }
}
