namespace FitnessTracker.DTOs.Responses.Activity
{
    public class UserActivityResponseDTO
    {
        public int UniqueSplitsCount { get; set; }
        public int UniqueWorkoutsCount { get; set; }
        public UserActivityWorkoutStatsResponseDTO MostCompletedWorkout { get; set; } = null!;
    }
}
