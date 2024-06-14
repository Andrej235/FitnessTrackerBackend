namespace FitnessTracker.DTOs.Requests.User
{
    public class RegisterUserRequestDTO
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
