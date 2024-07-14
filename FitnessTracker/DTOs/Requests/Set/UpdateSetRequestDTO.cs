namespace FitnessTracker.DTOs.Requests.Set
{
    public class UpdateSetRequestDTO
    {
        public int ExerciseId { get; set; }
        public int TopRepRange { get; set; }
        public int BottomRepRange { get; set; }
        public int RiR { get; set; }
    }
}
