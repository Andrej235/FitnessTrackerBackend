using FitnessTracker.DTOs.Requests.Workout;
using FitnessTracker.Models;

namespace FitnessTracker.Services.ModelServices.WorkoutService
{
    public partial class WorkoutService
    {
        public async Task<Guid> CreateComment(Guid userId, Guid workoutId, CreateWorkoutCommentRequestDTO request)
        {
            WorkoutComment mapped = commentCreateRequestMapper.Map(request);
            mapped.CreatorId = userId;
            mapped.WorkoutId = workoutId;

            WorkoutComment newComment = await commentCreateService.Add(mapped);
            return newComment.Id;
        }

        public async Task<Guid> CreateReply(Guid userId, Guid workoutId, Guid commentId, CreateWorkoutCommentRequestDTO request)
        {
            WorkoutComment mapped = commentCreateRequestMapper.Map(request);
            mapped.CreatorId = userId;
            mapped.WorkoutId = workoutId;
            mapped.ParentId = commentId;

            WorkoutComment newComment = await commentCreateService.Add(mapped);
            return newComment.Id;
        }
    }
}
