using FitnessTracker.DTOs.Responses.Set;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.SetMappers
{
    public class DetailedSetResponseMapper : IResponseMapper<Set, DetailedSetResponseDTO>
    {
        public DetailedSetResponseDTO Map(Set from)
        {
            return new()
            {
                Id = from.Id,
                NumberOfSets = from.NumberOfSets,
                BottomRepRange = from.BottomRepRange,
                TopRepRange = from.TopRepRange,
            };
        }
    }
}
