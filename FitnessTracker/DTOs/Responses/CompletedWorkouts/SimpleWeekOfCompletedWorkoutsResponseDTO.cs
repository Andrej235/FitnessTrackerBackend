namespace FitnessTracker.DTOs.Responses.CompletedWorkouts
{
    public class SimpleWeekOfCompletedWorkoutsResponseDTO
    {
        public DateTime StartDate { get; set; }
        public int CompletedCount { get; set; }
        public int TotalCount { get; set; }
    }
}
