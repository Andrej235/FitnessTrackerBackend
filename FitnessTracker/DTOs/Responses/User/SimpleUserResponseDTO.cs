namespace FitnessTracker.DTOs.Responses.User
{
    public class SimpleUserResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
    }
}
