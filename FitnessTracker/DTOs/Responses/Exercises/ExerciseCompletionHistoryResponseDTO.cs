namespace FitnessTracker.DTOs.Responses.Exercises
{
    public class ExerciseCompletionHistoryResponseDTO
    {
        public IEnumerable<ExerciseHistorySegmentResponseDTO> History { get; set; } = null!;
    }
}
