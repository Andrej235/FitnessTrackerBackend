namespace FitnessTracker.Models
{
    public class Split
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public Guid CreatorId { get; set; }
        public User Creator { get; set; } = null!;

        public ICollection<SplitWorkout> Workouts { get; set; } = [];
        public ICollection<User> Likes { get; set; } = [];
        public ICollection<User> Favorites { get; set; } = [];
        public ICollection<SplitComment> Comments { get; set; } = [];
    }
}
