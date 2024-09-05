namespace FitnessTracker.DTOs.Requests.User
{
    public class CreatePinsRequestDTO
    {
        public IEnumerable<Guid> NewPinIds { get; set; } = null!;
    }
}
