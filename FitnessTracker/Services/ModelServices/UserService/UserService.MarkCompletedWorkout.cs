using FitnessTracker.DTOs.Requests.Completed;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public partial class UserService
    {
        public async Task MarkCompletedWorkout(Guid userId, CreateCompletedWorkoutRequestDTO request)
        {
            DayOfWeek today = DateTime.Today.DayOfWeek;
            User? user = await readSingleService.Get(
                x => x.Id == userId,
                x => x.Include(x => x.CurrentSplit!).ThenInclude(x => x.Workouts.Where(w => w.Day == today)))
                ?? throw new UnauthorizedException();

            if (user.CurrentSplit is null)
                throw new NotFoundException("User does not have a selected split");

            Models.CompletedWorkout newCompletedWorkout = createCompletedWorkoutRequestMapper.Map(request);
            newCompletedWorkout.UserId = userId;
            newCompletedWorkout.SplitId = user.CurrentSplit.Id;
            newCompletedWorkout.WorkoutId = user.CurrentSplit.Workouts.First().WorkoutId;
            foreach (Models.CompletedSet set in newCompletedWorkout.CompletedSets)
                set.UserId = userId;

            _ = await completedWorkoutCreateService.Add(newCompletedWorkout);
        }
    }
}
