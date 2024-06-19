using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.UserMappers
{
    public class DetailedUserResponseMapper : IResponseMapper<User, DetailedUserResponseDTO>
    {
        public DetailedUserResponseDTO Map(User from)
        {
            return new()
            {
                Name = from.Name,
                Image = from.ProfilePic,
                Followers = from.Followers.Count(),
                Following = from.Following.Count(),
                CompletedWorkouts = from.CompletedWorkouts.Count()
            };
        }
    }
}
