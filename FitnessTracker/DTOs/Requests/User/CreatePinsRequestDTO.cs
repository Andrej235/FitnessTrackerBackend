namespace FitnessTracker.DTOs.Requests.User
{
    public class CreatePinsRequestDTO
    {
        public IEnumerable<CreateSinglePinRequestDTO> NewPins { get; set; } = null!;
    }
}
