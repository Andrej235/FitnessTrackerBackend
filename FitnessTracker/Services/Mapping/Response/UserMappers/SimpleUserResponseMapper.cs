using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.UserMappers
{
    public class SimpleUserResponseMapper : IResponseMapper<User, SimpleUserResponseDTO>
    {
        public SimpleUserResponseDTO Map(User from) => new()
        {
            Name = from.Name,
            Username = from.Username,
            Image = from.ProfilePic
        };
    }
}
