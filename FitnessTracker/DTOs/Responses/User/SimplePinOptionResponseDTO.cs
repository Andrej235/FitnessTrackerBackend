using static FitnessTracker.DTOs.Responses.User.SimplePinResponseDTO;

namespace FitnessTracker.DTOs.Responses.User
{
    public class SimplePinOptionResponseDTO
    {
        public string Name { get; set; } = null!;
        public int LikeCount { get; set; }
        public PinType Type { get; set; }
    }
}
