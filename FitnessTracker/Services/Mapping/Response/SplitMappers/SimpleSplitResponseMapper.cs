using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.SplitMappers
{
    public class SimpleSplitResponseMapper(IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper) : IResponseMapper<Split, SimpleSplitResponseDTO>
    {
        private readonly IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper = userResponseMapper;

        public SimpleSplitResponseDTO Map(Split from)
        {
            return new()
            {
                Id = from.Id,
                Name = from.Name,
                Creator = userResponseMapper.Map(from.Creator),
                IsPublic = from.IsPublic
            };
        }
    }
}
