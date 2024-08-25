using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.UserMappers
{
    public class DetailedPublicUserResponseMapper : IResponseMapper<User, DetailedPublicUserResponseDTO>
    {
        public DetailedPublicUserResponseDTO Map(User from) => new()
        {
            Name = from.Name,
            Username = from.Username,
            Bio = from.Bio,
            Image = from.ProfilePic,
            JoinedAt = from.JoinedAt,
        };
    }
}
