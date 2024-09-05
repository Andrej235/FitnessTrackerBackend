using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.UserMappers
{
    public class SimpleSplitPinResponseMapper : IResponseMapper<Split, SimplePinResponseDTO>
    {
        public SimplePinResponseDTO Map(Split from) => new()
        {
            Id = from.Id,
            Name = from.Name,
            Description = from.Description,
            Type = SimplePinResponseDTO.PinType.Split,
        };
    }
}
