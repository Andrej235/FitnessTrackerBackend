namespace FitnessTracker.DTOs.Requests.Split
{
    public class CreateSplitWorkoutRequestDTO
    {
        public Guid WorkoutId { get; set; }
        public DayOfWeek Day { get; set; } = DayOfWeek.Monday;
    }
}
