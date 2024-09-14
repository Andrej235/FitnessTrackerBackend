using FitnessTracker.DTOs.Responses.CompletedWorkouts;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;
using FitnessTracker.Utilities;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public partial class UserService
    {
        public async Task<IEnumerable<SimpleWeekOfCompletedWorkoutsResponseDTO>> GetStreak(Guid userId, int? year)
        {
            IEnumerable<IGrouping<DateTime, Models.CompletedWorkout>> groupedCompletedWorkouts = (await completedWorkoutReadRangeService.Get(
                criteria: x => x.UserId == userId,
                queryBuilder: x => x.Include(x => x.Split).ThenInclude(x => x.Workouts)))
                .GroupBy(x => x.CompletedAt.GetStartOfWeek());

            if (year is null)
            {
                DateTime startOfWeek = DateTime.Now.GetStartOfWeek();
                DateTime startOfLastYearsWeek = startOfWeek.AddYears(-1).GetStartOfWeek().AddDays(7);
                groupedCompletedWorkouts = groupedCompletedWorkouts.Where(x => x.Key > startOfLastYearsWeek && x.Key <= startOfWeek);
            }
            else
                groupedCompletedWorkouts = groupedCompletedWorkouts.Where(x => x.Key.Year == year);

            IEnumerable<SimpleWeekOfCompletedWorkoutsResponseDTO> mapped = groupedCompletedWorkouts.Select(simpleWeekOfCompletedWorkoutsResponseMapper.Map);
            return mapped;
        }

        public async Task<IEnumerable<SimpleWeekOfCompletedWorkoutsResponseDTO>> GetStreak(string username, int? year)
        {
            User? user = await readSingleService.Get(x => x.Username == username, x => x.Include(x => x.Settings)) ?? throw new NotFoundException($"User '{username}' not found");

            if (!user.Settings.PublicStreak)
                throw new AccessDeniedException("User's streak is not public");

            IEnumerable<IGrouping<DateTime, Models.CompletedWorkout>> groupedCompletedWorkouts = (await completedWorkoutReadRangeService.Get(
                criteria: x => x.UserId == user.Id,
                queryBuilder: x => x.Include(x => x.Split).ThenInclude(x => x.Workouts)))
                .GroupBy(x => x.CompletedAt.GetStartOfWeek());

            if (year is null)
            {
                DateTime startOfWeek = DateTime.Now.GetStartOfWeek();
                DateTime startOfLastYearsWeek = startOfWeek.AddYears(-1).GetStartOfWeek().AddDays(7);
                groupedCompletedWorkouts = groupedCompletedWorkouts.Where(x => x.Key > startOfLastYearsWeek && x.Key <= startOfWeek);
            }
            else
                groupedCompletedWorkouts = groupedCompletedWorkouts.Where(x => x.Key.Year == year);

            IEnumerable<SimpleWeekOfCompletedWorkoutsResponseDTO> mapped = groupedCompletedWorkouts.Select(simpleWeekOfCompletedWorkoutsResponseMapper.Map);
            return mapped;
        }

        public async Task<DetailedWeekOfCompletedWorkoutsResponseDTO> GetUserStreakOnWeek(Guid userId, DateTime date)
        {
            DateTime startOfWeek = date.GetStartOfWeek();
            DateTime endOfWeek = startOfWeek.Date.AddDays(7);
            IEnumerable<Models.CompletedWorkout> completedWorkout = await completedWorkoutReadRangeService.Get(
                criteria: x => x.UserId == userId && x.CompletedAt >= startOfWeek && x.CompletedAt <= endOfWeek,
                queryBuilder: x => x.Include(x => x.Split)
                    .ThenInclude(x => x.Workouts)
                    .ThenInclude(x => x.Workout)
                    .ThenInclude(x => x.Creator)
                    .Include(x => x.Split)
                    .ThenInclude(x => x.Creator));

            DetailedWeekOfCompletedWorkoutsResponseDTO mapped = detailedWeekOfCompletedWorkoutsResponseMapper.Map(completedWorkout);
            return mapped;
        }
    }
}
