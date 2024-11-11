namespace FitnessTracker.DTOs.Responses.Workout
{
    public class WorkoutExerciseChartSingleSetDataResponseDTO
    {
        public float WeightUsed { get; set; }
        public int RepsCompleted { get; set; }
        public DateTime TimeCompleted { get; set; }
    }
}
