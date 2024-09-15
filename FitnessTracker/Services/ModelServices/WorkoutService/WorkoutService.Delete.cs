using FitnessTracker.Exceptions;

namespace FitnessTracker.Services.ModelServices.WorkoutService
{
    public partial class WorkoutService
    {
        public async Task Delete(Guid userId, Guid id)
        {
            var workout = await readSingleSelectedService.Get(
                x => new
                {
                    x.CreatorId,
                },
                x => x.Id == id)
                ?? throw new NotFoundException();

            if (workout.CreatorId == userId)
                throw new AccessDeniedException();

            await likeDeleteService.Delete(x => x.WorkoutId == id);
            await favoriteDeleteService.Delete(x => x.WorkoutId == id);
            await commentLikeDeleteService.Delete(x => x.WorkoutId == id);
            await commentDeleteService.Delete(x => x.WorkoutId == id);
            await deleteService.Delete(x => x.CreatorId == userId && x.Id == id);
        }
    }
}
