using FitnessTracker.DTOs.Responses.User;

namespace FitnessTracker.DTOs.Responses.Split
{
    public class SimpleSplitWorkoutResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsPublic { get; set; }
        public SimpleUserResponseDTO Creator { get; set; } = null!;
        public DayOfWeek Day { get; set; }
    }
}