namespace FitnessTracker.DTOs.Responses.Workout
{
    public class NewWorkoutResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsPublic { get; set; }
    }
}
