namespace FitnessTracker.DTOs.Responses.Activity
{
    public class UserActivityWorkoutStatsResponseDTO
    {
        public ActivityWorkoutPreviewResponseDTO Workout { get; set; } = null!;
        public int CompletionCount { get; set; }
    }
}
