namespace FitnessTracker.DTOs.Requests.Pins
{
    public class ReorderPinsRequestDTO
    {
        public IEnumerable<UpdateSinglePinRequestDTO> NewOrder { get; set; } = null!;
    }
}
