namespace FitnessTracker.Models
{
    public class Workout
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public User Creator { get; set; } = null!;
        public Guid CreatorId { get; set; }

        public ICollection<Set> Sets { get; set; } = [];
        public ICollection<User> Likes { get; set; } = [];
        public ICollection<User> Favorites { get; set; } = [];
        public ICollection<WorkoutComment> Comments { get; set; } = [];
    }
}
