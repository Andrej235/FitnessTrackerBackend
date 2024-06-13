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

        public Guid? SplitId { get; set; }
        public Split? Split { get; set; }
    }
}
