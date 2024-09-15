using FitnessTracker.DTOs.Requests.Workout;
using FitnessTracker.Models;

namespace FitnessTracker.Services.ModelServices.WorkoutService
{
    public partial class WorkoutService
    {
        public async Task<string> CreateComment(Guid userId, Guid workoutId, CreateWorkoutCommentRequestDTO request)
        {
            WorkoutComment mapped = commentCreateRequestMapper.Map(request);
            mapped.CreatorId = userId;
            mapped.WorkoutId = workoutId;

            WorkoutComment newComment = await commentCreateService.Add(mapped);
            return newComment.Id.ToString();
        }

        public async Task<string> CreateReply(Guid userId, Guid workoutId, Guid commentId, CreateWorkoutCommentRequestDTO request)
        {
            WorkoutComment mapped = commentCreateRequestMapper.Map(request);
            mapped.CreatorId = userId;
            mapped.WorkoutId = workoutId;
            mapped.ParentId = commentId;

            WorkoutComment newComment = await commentCreateService.Add(mapped);
            return newComment.Id.ToString();
        }
    }
}
