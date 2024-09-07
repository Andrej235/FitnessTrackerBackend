using FitnessTracker.DTOs.Enums;

namespace FitnessTracker.DTOs.Responses.User
{
    public class SimplePinResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public PinType Type { get; set; }
        public int Order { get; set; }
    }
}
