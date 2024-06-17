namespace FitnessTracker.DTOs.Requests.Muscle
{
    public class CreateMuscleRequestDTO
    {
        public string Name { get; set; } = null!;
        public int MuscleGroupId { get; set; }
    }
}
