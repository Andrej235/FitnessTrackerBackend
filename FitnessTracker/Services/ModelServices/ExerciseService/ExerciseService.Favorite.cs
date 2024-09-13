using FitnessTracker.Models;

namespace FitnessTracker.Services.ModelServices.ExerciseService
{
    public partial class ExerciseService
    {
        public async Task CreateFavorite(int exerciseId, Guid userId) => _ = await favoriteExerciseCreateService.Add(new FavoriteExercise
        {
            UserId = userId,
            ExerciseId = exerciseId
        });

        public async Task DeleteFavorite(int exerciseId, Guid userId) => await favoriteExerciseDeleteService.Delete(x => x.UserId == userId && x.ExerciseId == exerciseId);
    }
}
