using FitnessTracker.DTOs.Requests.Split;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Services.ModelServices.SplitService
{
    public partial class SplitService
    {
        public async Task UpdateBasicInformation(Guid splitId, Guid userId, UpdateSplitBaseInfoRequestDTO request)
        {
            Split split = await readSingleService.Get(x => x.Id == splitId, x => x.Include(x => x.Workouts).ThenInclude(x => x.Workout)) ?? throw new NotFoundException($"Split with id {splitId} not found.");
            if (split.CreatorId != userId)
                throw new UnauthorizedAccessException("You can only update splits that you created.");

            split.Name = request.Name;
            split.Description = request.Description;
            split.IsPublic = request.IsPublic && split.Workouts.All(x => x.Workout.IsPublic);

            await updateService.Update(split);
        }

        public async Task UpdateSplitWorkout(Guid splitId, Guid userId, DayOfWeek day, [FromBody] UpdateSplitWorkoutRequestDTO request)
        {
            Split? split = await readSingleService.Get(x => x.Id == splitId, x => x.Include(x => x.Workouts).ThenInclude(x => x.Workout)) ?? throw new NotFoundException($"Split with id {splitId} not found.");
            if (split.CreatorId != userId)
                throw new UnauthorizedAccessException("You can only update splits that you created.");

            SplitWorkout splitWorkout = split.Workouts.FirstOrDefault(x => x.Day == day) ?? throw new NotFoundException($"Split workout for day {day} not found.");
            splitWorkout.WorkoutId = request.NewWorkoutId; //TODO: Check if workout is public

            await splitWorkoutUpdateService.Update(splitWorkout);
        }
    }
}
