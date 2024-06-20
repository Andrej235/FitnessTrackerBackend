using FitnessTracker.DTOs.Requests.Set;

namespace FitnessTracker.DTOs.Requests.Workout
{
    public class CreateWorkoutRequestDTO
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsPublic { get; set; }
        public IEnumerable<CreateSetRequestDTO> Sets { get; set; } = [];
    }
}
