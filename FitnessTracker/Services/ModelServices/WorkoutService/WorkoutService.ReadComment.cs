using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Services.Read;

namespace FitnessTracker.Services.ModelServices.WorkoutService
{
    public partial class WorkoutService
    {
        public Task<IEnumerable<SimpleWorkoutCommentResponseDTO>> GetComments(Guid workoutId, Guid? userId, int? offset, int? limit) => commentSelectService.Get(
                x => new SimpleWorkoutCommentResponseDTO
                {
                    Id = x.Id,
                    Text = x.Text,
                    Creator = new DTOs.Responses.User.SimpleUserResponseDTO
                    {
                        Username = x.Creator.Username,
                        Name = x.Creator.Name,
                        Image = x.Creator.ProfilePic,
                    },
                    CreatedAt = x.CreatedAt,
                    LikeCount = x.Likes.Count,
                    IsLiked = userId != null && x.Likes.Any(x => x.Id == userId),
                    IsCreator = userId != null && x.CreatorId == userId,
                    ReplyCount = x.Children.Count,
                    WorkoutId = x.WorkoutId,
                },
                x => x.WorkoutId == workoutId && x.ParentId == null,
                offset ?? 0,
                limit ?? 10,
                x => x.Include(x => x.Creator));

        public Task<IEnumerable<SimpleWorkoutCommentResponseDTO>> GetReplies(Guid workoutId, Guid commentId, Guid? userId, int? offset, int? limit) => commentSelectService.Get(
                x => new SimpleWorkoutCommentResponseDTO
                {
                    Id = x.Id,
                    Text = x.Text,
                    Creator = new DTOs.Responses.User.SimpleUserResponseDTO
                    {
                        Username = x.Creator.Username,
                        Name = x.Creator.Name,
                        Image = x.Creator.ProfilePic,
                    },
                    CreatedAt = x.CreatedAt,
                    LikeCount = x.Likes.Count,
                    IsLiked = userId != null && x.Likes.Any(x => x.Id == userId),
                    IsCreator = userId != null && x.CreatorId == userId,
                    ReplyCount = x.Children.Count,
                    WorkoutId = x.WorkoutId,
                },
                x => x.WorkoutId == workoutId && x.ParentId == commentId,
                offset ?? 0,
                limit ?? 10,
                x => x.Include(x => x.Creator));
    }
}
