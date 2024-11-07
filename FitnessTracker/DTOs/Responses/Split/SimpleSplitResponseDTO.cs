using FitnessTracker.DTOs.Responses.User;

namespace FitnessTracker.DTOs.Responses.Split
{
    public class SimpleSplitResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public SimpleUserResponseDTO Creator { get; set; } = null!;
    }
}
