namespace FitnessTracker.DTOs.Responses.User
{
    public class SimpleUserResponseDTO
    {
        public string Username { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
    }
}
