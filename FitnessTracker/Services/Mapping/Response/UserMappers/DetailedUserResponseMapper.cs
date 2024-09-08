using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.UserMappers
{
    public class DetailedUserResponseMapper(IResponseMapper<Split, DetailedUserSplitResponseDTO> splitResponseMapper) : IResponseMapper<User, DetailedUserResponseDTO>
    {
        private readonly IResponseMapper<Split, DetailedUserSplitResponseDTO> splitResponseMapper = splitResponseMapper;

        public DetailedUserResponseDTO Map(User from) => new()
        {
            Name = from.Name,
            Username = from.Username,
            Bio = from.Bio,
            Image = from.ProfilePic,
            CurrentSplit = from.CurrentSplit is null ? null : splitResponseMapper.Map(from.CurrentSplit),
            JoinedAt = from.JoinedAt,
            Gender = from.Gender,
        };
    }
}
