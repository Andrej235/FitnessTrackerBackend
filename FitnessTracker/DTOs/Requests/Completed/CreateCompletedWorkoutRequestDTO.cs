namespace FitnessTracker.DTOs.Requests.Completed
{
    public class CreateCompletedWorkoutRequestDTO
    {
        public IEnumerable<CreateCompletedSetRequestDTO> CompletedSets { get; set; } = null!;
    }
}
