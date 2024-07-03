using FitnessTracker.DTOs.Responses.CompletedWorkouts;
using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Models;
using FitnessTracker.Utilities;

namespace FitnessTracker.Services.Mapping.Response.UserMappers
{
    public class DetailedPublicUserResponseMapper(IResponseMapper<IGrouping<DateTime, CompletedWorkout>, SimpleWeekOfCompletedWorkoutsResponseDTO> simpleWeekOfCompletedWorkoutsResponseMapper) : IResponseMapper<User, DetailedPublicUserResponseDTO>
    {
        private readonly IResponseMapper<IGrouping<DateTime, CompletedWorkout>, SimpleWeekOfCompletedWorkoutsResponseDTO> simpleWeekOfCompletedWorkoutsResponseMapper = simpleWeekOfCompletedWorkoutsResponseMapper;

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
                JoinedAt = from.JoinedAt,
                Streak = from.CompletedWorkouts.GroupBy(x => x.CompletedAt.GetStartOfWeek()).Take(52).Select(simpleWeekOfCompletedWorkoutsResponseMapper.Map),
            };
        }
    }
}
