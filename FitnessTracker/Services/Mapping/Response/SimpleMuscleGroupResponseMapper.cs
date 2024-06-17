using FitnessTracker.DTOs.Responses.MuscleGroup;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response
{
    public class SimpleMuscleGroupResponseMapper : IResponseMapper<MuscleGroup, SimpleMuscleGroupResponseDTO>
    {
        public SimpleMuscleGroupResponseDTO Map(MuscleGroup from)
        {
            return new()
            {
                Id = from.Id,
                Name = from.Name,
            };
        }
    }
}
