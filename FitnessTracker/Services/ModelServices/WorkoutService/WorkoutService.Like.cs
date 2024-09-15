using FitnessTracker.Models;

namespace FitnessTracker.Services.ModelServices.WorkoutService
{
    public partial class WorkoutService
    {
        public Task CreateLike(Guid userId, Guid workoutId) => likeCreateService.Add(new WorkoutLike
        {
            UserId = userId,
            WorkoutId = workoutId,
            LikedAt = DateTime.Now
        });

        public Task DeleteLike(Guid userId, Guid workoutId) => likeDeleteService.Delete(x => x.UserId == userId && x.WorkoutId == workoutId);
    }
}
