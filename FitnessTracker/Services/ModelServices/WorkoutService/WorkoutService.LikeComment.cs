using FitnessTracker.Models;

namespace FitnessTracker.Services.ModelServices.WorkoutService
{
    public partial class WorkoutService
    {
        public Task CreateCommentLike(Guid userId, Guid commentId, Guid workoutId) => commentLikeCreateService.Add(new WorkoutCommentLike
        {
            UserId = userId,
            WorkoutCommentId = commentId,
            WorkoutId = workoutId
        });

        public Task DeleteCommentLike(Guid userId, Guid commentId) => commentLikeDeleteService.Delete(x => x.UserId == userId && x.WorkoutCommentId == commentId);
    }
}
