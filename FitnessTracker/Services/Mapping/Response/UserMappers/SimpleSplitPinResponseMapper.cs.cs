using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.UserMappers
{
    public class SimpleSplitPinResponseMapper : IResponseMapper<SplitPin, SimplePinResponseDTO>
    {
        public SimplePinResponseDTO Map(SplitPin from) => new()
        {
            Id = from.Split.Id,
            Name = from.Split.Name,
            Description = from.Split.Description,
            Type = SimplePinResponseDTO.PinType.Split,
            Order = from.Order,
        };
    }
}
