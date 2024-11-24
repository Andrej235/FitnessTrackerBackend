using FitnessTracker.DTOs.Responses.CompletedWorkouts;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;
using FitnessTracker.Utilities;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public partial class UserService
    {
        public async Task<IEnumerable<SimpleWeekOfCompletedWorkoutsResponseDTO>> GetStreak(string username, Guid? userId, int? year)
        {
            var user = await readSingleSelectedService.Get(
                x => new
                {
                    x.Id,
                    x.Settings.PublicStreak
                },
                x => x.Username == username) ?? throw new NotFoundException($"User '{username}' not found");

            if (!user.PublicStreak && userId != user.Id)
                throw new AccessDeniedException("User's streak is not public");

            IEnumerable<IGrouping<DateTime, CompletedWorkout>> groupedCompletedWorkouts = (await completedWorkoutReadRangeService.Get(
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

        public async Task<DetailedWeekOfCompletedWorkoutsResponseDTO> GetUserStreakOnWeek(string username, Guid? userId, DateTime date)
        {
            var user = await readSingleSelectedService.Get(x => new
            {
                x.Id,
                x.Settings.PublicStreak
            },
            x => x.Username == username) ?? throw new NotFoundException($"User '{username}' not found");

            if (!user.PublicStreak && userId != user.Id)
                throw new AccessDeniedException("User's streak is not public");

            DateTime startOfWeek = date.GetStartOfWeek();
            DateTime endOfWeek = startOfWeek.Date.AddDays(7);
            IEnumerable<CompletedWorkout> completedWorkout = await completedWorkoutReadRangeService.Get(
                criteria: x => x.UserId == user.Id && x.CompletedAt >= startOfWeek && x.CompletedAt <= endOfWeek,
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
