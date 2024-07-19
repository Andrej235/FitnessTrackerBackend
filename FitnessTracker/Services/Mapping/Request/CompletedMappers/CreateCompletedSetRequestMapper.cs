using FitnessTracker.DTOs.Requests.Completed;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Request.CompletedMappers
{
    public class CreateCompletedSetRequestMapper : IRequestMapper<CreateCompletedSetRequestDTO, CompletedSet>
    {
        public CompletedSet Map(CreateCompletedSetRequestDTO from) => new()
        {
            WeightUsed = from.WeightUsed,
            RepsCompleted = from.RepsCompleted,
            SetId = from.SetId
        };
    }
}
