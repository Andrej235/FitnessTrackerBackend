using FitnessTracker.Exceptions;

namespace FitnessTracker.Services.ModelServices.WorkoutService
{
    public partial class WorkoutService
    {
        public async Task Delete(Guid userId, string creatorUsername, string workoutName)
        {
            Guid creatorId = (await userReadSingleSelectedService.Get(x => new { x.Id }, x => x.Username == creatorUsername)
                ?? throw new NotFoundException($"User {creatorUsername} not found")).Id;

            if (creatorId != userId)
                throw new AccessDeniedException();

            var workout = await readSingleSelectedService.Get(
                x => new
                {
                    x.Id,
                },
                x => x.CreatorId == creatorId && x.Name == workoutName)
                ?? throw new NotFoundException();

            await likeDeleteService.Delete(x => x.WorkoutId == workout.Id, false);
            await favoriteDeleteService.Delete(x => x.WorkoutId == workout.Id, false);
            await commentLikeDeleteService.Delete(x => x.WorkoutId == workout.Id, false);
            await commentDeleteService.Delete(x => x.WorkoutId == workout.Id, false);
            await deleteService.Delete(x => x.CreatorId == userId && x.Id == workout.Id, false);
        }
    }
}
