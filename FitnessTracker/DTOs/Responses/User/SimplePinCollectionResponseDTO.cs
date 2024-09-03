namespace FitnessTracker.DTOs.Responses.User
{
    public class SimplePinCollectionResponseDTO
    {
        public IEnumerable<SimplePinResponseDTO> Pins { get; set; } = null!;
    }
}
