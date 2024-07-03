namespace FitnessTracker.DTOs.Requests.Workout
{
    public class UpdateWorkoutBaseInfoRequestDTO
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsPublic { get; set; }
    }
}
