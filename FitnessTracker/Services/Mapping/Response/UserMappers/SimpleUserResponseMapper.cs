using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.UserMappers
{
    public class SimpleUserResponseMapper : IResponseMapper<User, SimpleUserResponseDTO>
    {
        public SimpleUserResponseDTO Map(User from)
        {
            return new()
            {
                Id = from.Id,
                Name = from.Name,
                Image = from.ProfilePic
            };
        }
    }
}
