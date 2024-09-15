using FitnessTracker.Exceptions;

namespace FitnessTracker.Services.ModelServices.WorkoutService
{
    public partial class WorkoutService
    {
        public async Task DeleteComment(Guid userId, Guid workoutId, Guid commentId)
        {
            var comment = await commentReadSingleSelectedService.Get(
                x => new
                {
                    x.CreatorId
                },
                x => x.WorkoutId == workoutId && x.Id == commentId)
                ?? throw new NotFoundException();

            if (comment.CreatorId != userId)
                throw new AccessDeniedException();

            await commentDeleteService.Delete(x => x.WorkoutId == workoutId && x.ParentId == commentId);
            await commentDeleteService.Delete(x => x.WorkoutId == workoutId && x.CreatorId == userId && x.Id == commentId);
        }
    }
}
