namespace FitnessTracker.Models
{
    public class Workout
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsPublic { get; set; }

        public User Creator { get; set; } = null!;
        public Guid CreatorId { get; set; }

        public IEnumerable<Set> Sets { get; set; } = new HashSet<Set>();
        public IEnumerable<Split> Splits { get; set; } = new HashSet<Split>();
    }
}
