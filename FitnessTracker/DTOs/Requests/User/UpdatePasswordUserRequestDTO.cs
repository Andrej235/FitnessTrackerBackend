namespace FitnessTracker.DTOs.Requests.User
{
    public class UpdatePasswordUserRequestDTO
    {
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
