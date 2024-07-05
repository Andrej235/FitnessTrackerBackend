using FitnessTracker.DTOs.Requests.Completed;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Request.CompletedMappers
{
    public class CreateCompletedSetRequestMapper : IRequestMapper<CreteCompletedSetRequestDTO, CompletedSet>
    {
        public CompletedSet Map(CreteCompletedSetRequestDTO from)
        {
            return new()
            {
                WeightUsed = from.WeightUsed,
                RepsCompleted = from.RepsCompleted,
            };
        }
    }
}
