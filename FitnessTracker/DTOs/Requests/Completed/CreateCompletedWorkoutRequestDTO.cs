namespace FitnessTracker.DTOs.Requests.Completed
{
    public class CreateCompletedWorkoutRequestDTO
    {
        public IEnumerable<CreteCompletedSetRequestDTO> CompletedSets { get; set; } = [];
    }
}
