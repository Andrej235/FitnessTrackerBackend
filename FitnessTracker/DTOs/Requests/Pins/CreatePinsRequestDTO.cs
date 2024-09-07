namespace FitnessTracker.DTOs.Requests.Pins
{
    public class CreatePinsRequestDTO
    {
        public IEnumerable<CreateSinglePinRequestDTO> NewPins { get; set; } = null!;
    }
}
