using FitnessTracker.DTOs.Enums;

namespace FitnessTracker.DTOs.Responses.User
{
    public class SimplePinOptionResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int LikeCount { get; set; }
        public PinType Type { get; set; }
    }
}
