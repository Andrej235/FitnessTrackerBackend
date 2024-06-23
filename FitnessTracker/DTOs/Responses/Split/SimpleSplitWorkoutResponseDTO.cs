using FitnessTracker.DTOs.Responses.Workout;

namespace FitnessTracker.DTOs.Responses.Split
{
    public class SimpleSplitWorkoutResponseDTO
    {
        public SimpleWorkoutResponseDTO Workout { get; set; } = null!;
        public DayOfWeek Day { get; set; }
    }
}