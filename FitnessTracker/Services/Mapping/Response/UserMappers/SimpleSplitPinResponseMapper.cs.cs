using FitnessTracker.DTOs.Enums;
using FitnessTracker.DTOs.Responses.Pins;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.UserMappers
{
    public class SimpleSplitPinResponseMapper : IResponseMapper<SplitPin, PinResponseDTO>
    {
        public PinResponseDTO Map(SplitPin from) => new()
        {
            Id = from.Split.Id,
            Name = from.Split.Name,
            Description = from.Split.Description,
            Type = PinType.Split,
            Order = from.Order,
        };
    }
}
