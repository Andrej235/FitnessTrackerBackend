namespace FitnessTracker.DTOs.Responses.Workout
{
    public class WorkoutExerciseChartDataResponseDTO
    {
        public IEnumerable<WorkoutExerciseChartSingleSetDataResponseDTO> Data { get; set; } = null!;
    }
}
