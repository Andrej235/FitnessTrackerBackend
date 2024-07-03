using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.UserMappers
{
    public class DetailedPublicUserResponseMapper : IResponseMapper<User, DetailedPublicUserResponseDTO>
    {
        public DetailedPublicUserResponseDTO Map(User from)
        {
            return new()
            {
                Id = from.Id,
                Name = from.Name,
                Image = from.ProfilePic,
                Followers = from.Followers.Count,
                Following = from.Following.Count,
                TotalCompletedWorkouts = from.CompletedWorkouts.Count,
            };
        }
    }
}
