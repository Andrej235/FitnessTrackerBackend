namespace FitnessTracker.DTOs.Requests.Set
{
    public class CreateSetRequestDTO
    {
        public int ExerciseId { get; set; }
        public int TopRepRange { get; set; }
        public int BottomRepRange { get; set; }
        public float Intensity { get; set; }
        public Models.Set.SetType Type { get; set; }
    }
}
