using FitnessTracker.Data;
using FitnessTracker.Models;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Read;
using FitnessTracker.Utilities;
using System.Diagnostics;

namespace FitnessTracker.Services.Update
{
    public class ExerciseUpdateService(DataContext context,
                                       IReadService<Exercise> readService,
                                       IDeleteService<EquipmentUsage> equipmentUsageDeleteService,
                                       IDeleteService<PrimaryMuscleGroupInExercise> primaryMuscleGroupDeleteService,
                                       IDeleteService<SecondaryMuscleGroupInExercise> secondaryMuscleGroupDeleteService,
                                       IDeleteService<PrimaryMuscleInExercise> primaryMuscleDeleteService,
                                       IDeleteService<SecondaryMuscleInExercise> secondaryMuscleDeleteService) : IUpdateService<Exercise>
    {
        public async Task Update(Exercise updatedEntity)
        {
            try
            {
                var exercise = await readService.Get(x => x.Id == updatedEntity.Id, "none") ?? throw new NullReferenceException("Exercise was not found.");
                context.AttachRange(exercise);

                exercise.Name = updatedEntity.Name;
                exercise.Description = updatedEntity.Description;
                exercise.Image = updatedEntity.Image;

                await equipmentUsageDeleteService.DeleteAll(x => x.ExerciseId == exercise.Id);
                exercise.Equipment = updatedEntity.Equipment;

                await primaryMuscleGroupDeleteService.DeleteAll(x => x.ExerciseId == exercise.Id);
                exercise.PrimaryMuscleGroups = updatedEntity.PrimaryMuscleGroups;

                await secondaryMuscleGroupDeleteService.DeleteAll(x => x.ExerciseId == exercise.Id);
                exercise.SecondaryMuscleGroups = updatedEntity.SecondaryMuscleGroups;

                await primaryMuscleDeleteService.DeleteAll(x => x.ExerciseId == exercise.Id);
                exercise.PrimaryMuscles = updatedEntity.PrimaryMuscles;

                await secondaryMuscleDeleteService.DeleteAll(x => x.ExerciseId == exercise.Id);
                exercise.SecondaryMuscles = updatedEntity.SecondaryMuscles;

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ex.LogError();
            }
        }
    }
}
