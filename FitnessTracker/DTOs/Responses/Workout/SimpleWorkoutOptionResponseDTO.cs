using FitnessTracker.DTOs.Responses.User;

namespace FitnessTracker.DTOs.Responses.Workout
{
    public class SimpleWorkoutOptionResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsPublic { get; set; }
        public int LikeCount { get; set; }
        public SimpleUserResponseDTO Creator { get; set; } = null!;
    }
}
