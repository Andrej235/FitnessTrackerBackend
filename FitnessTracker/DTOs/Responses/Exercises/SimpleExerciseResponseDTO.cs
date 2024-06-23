namespace FitnessTracker.DTOs.Responses.Exercises
{
    public class SimpleExerciseResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
    }
}
