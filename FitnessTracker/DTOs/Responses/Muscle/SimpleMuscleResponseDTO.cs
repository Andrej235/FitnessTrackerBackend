namespace FitnessTracker.DTOs.Responses.Muscle
{
    public class SimpleMuscleResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int MuscleGroupId { get; set; }
    }
}
