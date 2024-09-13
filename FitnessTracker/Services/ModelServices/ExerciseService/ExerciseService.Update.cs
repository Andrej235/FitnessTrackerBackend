using FitnessTracker.DTOs.Requests.Exercise;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;

namespace FitnessTracker.Services.ModelServices.ExerciseService
{
    public partial class ExerciseService
    {
        public async Task Update(UpdateExerciseRequestDTO updatedExercise)
        {
            if (updatedExercise.Id <= 0)
                throw new BadRequestException("Exercise id must be greater than 0");

            if (string.IsNullOrWhiteSpace(updatedExercise.Name))
                throw new BadRequestException("Exercise name is required");

            Exercise? exercise = await readSingleService.Get(x => x.Id == updatedExercise.Id) ?? throw new NotFoundException($"Exercise with id {updatedExercise.Id} was not found");

            await equipmetUsageDeleteService.Delete(x => x.ExerciseId == updatedExercise.Id);
            await primaryMuscleGroupDeleteService.Delete(x => x.ExerciseId == updatedExercise.Id);
            await primaryMuscleDeleteService.Delete(x => x.ExerciseId == updatedExercise.Id);
            await secondaryMuscleGroupDeleteService.Delete(x => x.ExerciseId == updatedExercise.Id);
            await secondaryMuscleDeleteService.Delete(x => x.ExerciseId == updatedExercise.Id);

            await equipmetUsageCreateRangeService.Add(updatedExercise.Equipment.Select(x => new EquipmentUsage
            {
                ExerciseId = updatedExercise.Id,
                EquipmentId = x
            }));

            await primaryMuscleGroupCreateRangeService.Add(updatedExercise.PrimaryMuscleGroups.Select(x => new PrimaryMuscleGroupInExercise
            {
                ExerciseId = updatedExercise.Id,
                MuscleGroupId = x
            }));

            await primaryMuscleCreateRangeService.Add(updatedExercise.PrimaryMuscles.Select(x => new PrimaryMuscleInExercise
            {
                ExerciseId = updatedExercise.Id,
                MuscleId = x
            }));

            await secondaryMuscleGroupCreateRangeService.Add(updatedExercise.SecondaryMuscleGroups.Select(x => new SecondaryMuscleGroupInExercise
            {
                ExerciseId = updatedExercise.Id,
                MuscleGroupId = x
            }));

            await secondaryMuscleCreateRangeService.Add(updatedExercise.SecondaryMuscles.Select(x => new SecondaryMuscleInExercise
            {
                ExerciseId = updatedExercise.Id,
                MuscleId = x
            }));

            exercise.Name = updatedExercise.Name;
            exercise.Description = updatedExercise.Description;
            exercise.Image = updatedExercise.Image ?? "";
            await updateService.Update(exercise);
        }
    }
}
