using FitnessTracker.DTOs.Responses.Split;

namespace FitnessTracker.DTOs.Responses.CompletedWorkouts
{
    public class DetailedWeekOfCompletedWorkoutsResponseDTO
    {
        public DetailedUserSplitResponseDTO Split { get; set; } = null!;
        public IEnumerable<DayOfWeek> CompletedWorkouts { get; set; } = [];
    }
}
