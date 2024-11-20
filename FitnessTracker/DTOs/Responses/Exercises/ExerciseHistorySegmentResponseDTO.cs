using FitnessTracker.DTOs.Responses.CompletedWorkouts;

namespace FitnessTracker.DTOs.Responses.Exercises
{
    public class ExerciseHistorySegmentResponseDTO
    {
        public DateTime CompletionDate { get; set; }
        public IEnumerable<CompletedSetResponseDTO> SetsCompleted { get; set; } = null!;
    }
}
