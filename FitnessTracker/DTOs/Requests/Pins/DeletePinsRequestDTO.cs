namespace FitnessTracker.DTOs.Requests.Pins
{
    public class DeletePinsRequestDTO
    {
        public IEnumerable<DeleteSinglePinRequestDTO> DeletedPins { get; set; } = null!;
    }
}
