using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public partial class UserService
    {
        public async Task<DetailedWorkoutResponseDTO> GetTodaysWorkout(Guid userId)
        {
            DayOfWeek today = DateTime.Today.DayOfWeek;
            User? user = await readSingleService.Get(
                x => x.Id == userId,
                x => x.Include(x => x.CurrentSplit!)
                      .ThenInclude(x => x.Workouts.Where(w => w.Day == today))
                      .ThenInclude(x => x.Workout)
                      .ThenInclude(x => x.Sets)
                      .ThenInclude(x => x.Exercise))
                ?? throw new UnauthorizedException();

            if (user.CurrentSplit is null)
                throw new NotFoundException("User does not have a selected split");

            SplitWorkout workout = user.CurrentSplit.Workouts.FirstOrDefault() ?? throw new NotFoundException("User's split has no workouts for today");

            DetailedWorkoutResponseDTO mapped = detailedWorkoutResponseMapper.Map(workout.Workout);
            return mapped;
        }
    }
}
