using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.UserMappers
{
    public class DetailedUserResponseMapper(IResponseMapper<Split, DetailedUserSplitResponseDTO> splitResponseMapper) : IResponseMapper<User, DetailedUserResponseDTO>
    {
        private readonly IResponseMapper<Split, DetailedUserSplitResponseDTO> splitResponseMapper = splitResponseMapper;

        public DetailedUserResponseDTO Map(User from)
        {
            return new()
            {
                Name = from.Name,
                Image = from.ProfilePic,
                Followers = from.Followers.Count,
                Following = from.Following.Count,
                CompletedWorkouts = from.CompletedWorkouts.Count,
                CurrentSplit = from.CurrentSplit is null ? null : splitResponseMapper.Map(from.CurrentSplit)
            };
        }
    }
}
