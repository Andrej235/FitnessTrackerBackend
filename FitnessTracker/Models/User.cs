namespace FitnessTracker.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] Salt { get; set; } = null!;
        public string? ProfilePic { get; set; }

        public string Role { get; set; } = null!;

        public IEnumerable<Workout> CreatedWorkouts { get; set; } = new HashSet<Workout>();
        public IEnumerable<CompletedWorkout> CompletedWorkouts { get; set; } = new HashSet<CompletedWorkout>();
        public IEnumerable<Split> CreatedSplits { get; set; } = new HashSet<Split>();

        public Guid? SplitId { get; set; }
        public Split? CurrentSplit { get; set; }

        public IEnumerable<User> Following { get; set; } = new HashSet<User>();
        public IEnumerable<User> Followers { get; set; } = new HashSet<User>();
    }
}
