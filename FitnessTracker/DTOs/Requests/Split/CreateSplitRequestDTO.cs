namespace FitnessTracker.DTOs.Requests.Split
{
    public class CreateSplitRequestDTO
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public ICollection<CreateSplitWorkoutRequestDTO> Workouts { get; set; } = [];
    }
}
