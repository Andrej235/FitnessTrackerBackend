namespace FitnessTracker.DTOs.Responses.User
{
    public class SimplePinResponseDTO
    {
        public enum PinType
        {
            Workout,
            Split,
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public PinType Type { get; set; }
    }
}
