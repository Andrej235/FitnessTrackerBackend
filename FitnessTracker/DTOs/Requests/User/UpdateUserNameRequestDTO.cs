namespace FitnessTracker.DTOs.Requests.User
{
    /// <summary>
    /// Request DTO for updating User's name NOT username
    /// </summary>
    public class UpdateUserNameRequestDTO
    {
        public string NewName { get; set; } = null!;
    }
}
