using FitnessTracker.DTOs.Responses.User;

namespace FitnessTracker.DTOs.Responses.Workout
{
    public class SimpleWorkoutResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public SimpleUserResponseDTO Creator { get; set; } = null!;
    }
}
