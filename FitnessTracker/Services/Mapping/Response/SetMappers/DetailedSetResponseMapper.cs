using FitnessTracker.DTOs.Responses.Set;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.SetMappers
{
    public class DetailedSetResponseMapper : IResponseMapper<Set, DetailedSetResponseDTO>
    {
        public DetailedSetResponseDTO Map(Set from) => new()
        {
            Id = from.Id,
            BottomRepRange = from.BottomRepRange,
            TopRepRange = from.TopRepRange,
            ExerciseId = from.ExerciseId,
            Type = from.Type,
            RiR = from.RiR
        };
    }
}
