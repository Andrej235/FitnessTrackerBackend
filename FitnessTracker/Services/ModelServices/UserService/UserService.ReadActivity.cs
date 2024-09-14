using FitnessTracker.DTOs.Responses.Activity;
using FitnessTracker.Exceptions;
using FitnessTracker.Utilities;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public partial class UserService
    {
        public async Task<UserActivityResponseDTO> ReadLatestActivity(Guid userId)
        {
            if (userId == default)
                throw new UnauthorizedException();

            DateTime startOfWeek = DateTime.Now.GetStartOfWeek().AddDays(7);
            DateTime startOfLastYearsWeek = startOfWeek.AddYears(-1).GetStartOfWeek().AddDays(7);
            UserActivityResponseDTO? activity = await readSingleSelectedService.Get(
                select: x => new UserActivityResponseDTO
                {
                    UniqueSplitsCount = x.CompletedWorkouts
                        .Where(x => x.CompletedAt > startOfLastYearsWeek && x.CompletedAt <= startOfWeek)
                        .GroupBy(x => x.SplitId)
                        .Count(),
                    UniqueWorkoutsCount = x.CompletedWorkouts
                        .Where(x => x.CompletedAt > startOfLastYearsWeek && x.CompletedAt <= startOfWeek)
                        .GroupBy(x => x.WorkoutId != null ? x.WorkoutId : default)
                        .Count(),
                    MostCompletedWorkout = x.CompletedWorkouts
                        .Where(x => x.CompletedAt > startOfLastYearsWeek && x.CompletedAt <= startOfWeek)
                        .GroupBy(x => x.WorkoutId)
                        .Select(x => new UserActivityWorkoutStatsResponseDTO
                        {
                            Workout = x.Select(x => new ActivityWorkoutPreviewResponseDTO
                            {
                                Id = x.WorkoutId,
                                Name = x.Workout != null ? x.Workout.Name : null,
                            }).First(),
                            CompletionCount = x.Count(),
                        })
                        .OrderByDescending(x => x.CompletionCount)
                        .First(),
                },
                criteria: x => x.Id == userId)
                ?? throw new UnauthorizedException();

            return activity;
        }
    }
}
