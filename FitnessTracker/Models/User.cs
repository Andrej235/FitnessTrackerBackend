namespace FitnessTracker.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Bio { get; set; } = null!;
        public string Email { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] Salt { get; set; } = null!;
        public string? ProfilePic { get; set; }
        public DateTime JoinedAt { get; set; }

        public string Role { get; set; } = null!;

        public ICollection<Workout> CreatedWorkouts { get; set; } = [];
        public ICollection<CompletedWorkout> CompletedWorkouts { get; set; } = [];
        public ICollection<CompletedSet> CompletedSets { get; set; } = [];
        public ICollection<Split> CreatedSplits { get; set; } = [];

        public Guid? SplitId { get; set; }
        public Split? CurrentSplit { get; set; }

        public ICollection<UserFollows> Following { get; set; } = [];
        public ICollection<UserFollows> Followers { get; set; } = [];

        public bool EmailConfirmed { get; set; }
    }
}
