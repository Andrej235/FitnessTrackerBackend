namespace FitnessTracker.DTOs.Responses.Exercises
{
    public class BaseExerciseResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
    }
}
