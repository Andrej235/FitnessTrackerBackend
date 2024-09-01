using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.UserMappers
{
    public class UserProfilePictureResponseMapper : IResponseMapper<User, UserProfilePictureResponseDTO>
    {
        public UserProfilePictureResponseDTO Map(User from) => new()
        {
            Image = from.ProfilePic,
        };
    }
}
