using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.UserMappers
{
    public class DetailedPublicUserResponseMapper : IResponseMapper<User, DetailedPublicUserResponseDTO>
    {
        public DetailedPublicUserResponseDTO Map(User from) => new()
        {
            Id = from.Id,
            Name = from.Name,
            Image = from.ProfilePic,
            JoinedAt = from.JoinedAt,
        };
    }
}
