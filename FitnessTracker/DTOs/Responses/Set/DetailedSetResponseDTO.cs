
namespace FitnessTracker.DTOs.Responses.Set
{
    public class DetailedSetResponseDTO
    {
        public Guid Id { get; set; }
        public int TopRepRange { get; set; }
        public int BottomRepRange { get; set; }
        public int NumberOfSets { get; set; }
        public int ExerciseId { get; set; }
    }
}
