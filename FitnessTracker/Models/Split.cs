namespace FitnessTracker.Models
{
    public class Split
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public Guid CreatorId { get; set; }
        public User Creator { get; set; } = null!;

        public IEnumerable<Workout> Workouts { get; set; } = new HashSet<Workout>();
        public IEnumerable<User> Likes { get; set; } = new HashSet<User>();
        public IEnumerable<User> Favorites { get; set; } = new HashSet<User>();
        public IEnumerable<SplitComment> Comments { get; set; } = new HashSet<SplitComment>();
    }
}
