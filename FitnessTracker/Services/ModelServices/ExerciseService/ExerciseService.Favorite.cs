using FitnessTracker.Models;

namespace FitnessTracker.Services.ModelServices.ExerciseService
{
    public partial class ExerciseService
    {
        public Task CreateFavorite(int exerciseId, Guid userId) => favoriteExerciseCreateService.Add(new FavoriteExercise
        {
            UserId = userId,
            ExerciseId = exerciseId
        });

        public Task DeleteFavorite(int exerciseId, Guid userId) => favoriteExerciseDeleteService.Delete(x => x.UserId == userId && x.ExerciseId == exerciseId);
    }
}
