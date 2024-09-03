using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.UserMappers
{
    public class SimpleSplitPinResponseMapper(IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper) : IResponseMapper<Split, SimplePinResponseDTO>
    {
        private readonly IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper = userResponseMapper;

        public SimplePinResponseDTO Map(Split from) => new()
        {
            Id = from.Id,
            Name = from.Name,
            Description = from.Description,
            Creator = userResponseMapper.Map(from.Creator),
            Type = SimplePinResponseDTO.PinType.Split,
        };
    }
}
