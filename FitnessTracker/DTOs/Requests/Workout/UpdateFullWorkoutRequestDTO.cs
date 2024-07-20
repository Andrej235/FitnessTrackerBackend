using FitnessTracker.DTOs.Requests.Set;

namespace FitnessTracker.DTOs.Requests.Workout
{
    public class UpdateFullWorkoutRequestDTO : UpdateWorkoutBaseInfoRequestDTO
    {
        public IEnumerable<CreateSetRequestDTO> Sets { get; set; } = [];
    }
}
