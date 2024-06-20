using FitnessTracker.DTOs.Requests.Set;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Request.SetMappers
{
    public class CreateSetRequestMapper : IRequestMapper<CreateSetRequestDTO, Set>
    {
        public Set Map(CreateSetRequestDTO from)
        {
            return new Set
            {
                ExerciseId = from.ExerciseId,
                NumberOfSets = from.NumberOfSets,
                BottomRepRange = from.BottomRepRange,
                TopRepRange = from.TopRepRange,
            };
        }
    }
}
