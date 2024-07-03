using FitnessTracker.DTOs.Responses.Set;
using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.WorkoutMappers
{
    public class DetailedWorkoutResponseMapper(IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper,
                                               IResponseMapper<Set, DetailedSetResponseDTO> setResponseMapper) : IResponseMapper<Workout, DetailedWorkoutResponseDTO>
    {
        private readonly IResponseMapper<User, SimpleUserResponseDTO> userResponseMapper = userResponseMapper;
        private readonly IResponseMapper<Set, DetailedSetResponseDTO> setResponseMapper = setResponseMapper;

        public DetailedWorkoutResponseDTO Map(Workout from)
        {
            return new()
            {
                Id = from.Id,
                Name = from.Name,
                Description = from.Description,
                IsPublic = from.IsPublic,
                Creator = userResponseMapper.Map(from.Creator),
                Sets = from.Sets.Select(setResponseMapper.Map),
                CommentCount = from.Comments.Count,
                FavoriteCount = from.Favorites.Count,
                LikeCount = from.Likes.Count,
            };
        }
    }
}
