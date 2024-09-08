namespace FitnessTracker.DTOs.Requests.User
{
    public class UpdateUserEmailRequestDTO
    {
        public string OldEmail { get; set; } = null!;
        public string NewEmail { get; set; } = null!;
    }
}
