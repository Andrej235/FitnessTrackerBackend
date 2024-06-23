using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.SplitMappers
{
    public class DetailedSplitResponseMapper(IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper, IResponseMapper<SplitWorkout, SimpleSplitWorkoutResponseDTO> splitWorkoutResponseMapper) : IResponseMapper<Split, DetailedSplitResponseDTO>
    {
        private readonly IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper = userResponseMapper;
        private readonly IResponseMapper<SplitWorkout, SimpleSplitWorkoutResponseDTO> workoutResponseMapper = splitWorkoutResponseMapper;

        public DetailedSplitResponseDTO Map(Split from)
        {
            return new()
            {
                Id = from.Id,
                Name = from.Name,
                Description = from.Description,
                LikeCount = from.Likes.Count,
                FavoriteCount = from.Favorites.Count,
                CommentCount = from.Comments.Count,
                Creator = userResponseMapper.Map(from.Creator),
                Workouts = from.Workouts.Select(workoutResponseMapper.Map),
            };
        }
    }
}
