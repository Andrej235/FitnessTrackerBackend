using FitnessTracker.DTOs.Requests.Workout;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;

namespace FitnessTracker.Services.ModelServices.WorkoutService
{
    public partial class WorkoutService
    {
        public async Task UpdateWorkout(Guid userId, string creatorUsername, string name, UpdateFullWorkoutRequestDTO request)
        {
            Guid creatorId = (await userReadSingleSelectedService.Get(x => new { x.Id }, x => x.Username == creatorUsername)
                ?? throw new NotFoundException($"User {creatorUsername} not found")).Id;

            if (creatorId != userId)
                throw new AccessDeniedException();

            Workout? workout = await readSingleService.Get(x => x.CreatorId == creatorId && x.Name == name, x => x.Include(x => x.Sets)) ?? throw new NotFoundException();
            if (workout.CreatorId != userId)
                throw new AccessDeniedException();

            workout.Name = request.Name;
            workout.Description = request.Description;
            workout.Sets = request.Sets.Select(setCreateRequestMapper.Map).ToList();
            await updateService.Update(workout);
        }
    }
}
