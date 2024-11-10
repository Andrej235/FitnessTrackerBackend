using FitnessTracker.DTOs.Enums;

namespace FitnessTracker.DTOs.Responses.Pins
{
    public class PinResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public PinType Type { get; set; }
        public int Order { get; set; }
        public int LikeCount { get; set; }
        public int FavoriteCount { get; set; }
    }
}
