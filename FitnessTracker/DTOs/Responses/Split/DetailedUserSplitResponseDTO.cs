using FitnessTracker.DTOs.Responses.User;

namespace FitnessTracker.DTOs.Responses.Split
{
    public class DetailedUserSplitResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public IEnumerable<SimpleSplitWorkoutResponseDTO> Workouts { get; set; } = null!;
        public SimpleUserResponseDTO Creator { get; set; } = null!;
    }
}
