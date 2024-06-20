using FitnessTracker.DTOs.Responses.User;

namespace FitnessTracker.DTOs.Responses.Workout
{
    public class BaseWorkoutResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public SimpleUserResponseDTO Creator { get; set; } = null!;
    }
}
