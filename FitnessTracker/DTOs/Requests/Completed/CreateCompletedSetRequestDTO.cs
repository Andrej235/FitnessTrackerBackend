namespace FitnessTracker.DTOs.Requests.Completed
{
    public class CreateCompletedSetRequestDTO
    {
        public float WeightUsed { get; set; }
        public int RepsCompleted { get; set; }
        public Guid SetId { get; set; }
    }
}