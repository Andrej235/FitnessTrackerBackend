using FitnessTracker.Models;

namespace FitnessTracker.Services.ModelServices.WorkoutService
{
    public partial class WorkoutService
    {
        public Task CreateFavorite(Guid userId, Guid workoutId) => favoriteCreateService.Add(new FavoriteWorkout
        {
            UserId = userId,
            WorkoutId = workoutId,
            FavoritedAt = DateTime.Now
        });

        public Task DeleteFavorite(Guid userId, Guid workoutId) => favoriteDeleteService.Delete(x => x.UserId == userId && x.WorkoutId == workoutId);
    }
}
