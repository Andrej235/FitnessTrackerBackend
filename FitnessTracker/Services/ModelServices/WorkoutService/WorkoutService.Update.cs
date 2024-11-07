using FitnessTracker.DTOs.Requests.Workout;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;

namespace FitnessTracker.Services.ModelServices.WorkoutService
{
    public partial class WorkoutService
    {
        public async Task UpdateWorkout(Guid userId, Guid workoutId, UpdateFullWorkoutRequestDTO request)
        {
            Workout? workout = await readSingleService.Get(x => x.Id == workoutId, x => x.Include(x => x.Sets)) ?? throw new NotFoundException();
            if (workout.CreatorId != userId)
                throw new AccessDeniedException();

            workout.Name = request.Name;
            workout.Description = request.Description;
            workout.Sets = request.Sets.Select(setCreateRequestMapper.Map).ToList();
            await updateService.Update(workout);
        }
    }
}
