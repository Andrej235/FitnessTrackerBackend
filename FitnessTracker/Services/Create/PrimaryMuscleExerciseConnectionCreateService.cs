using FitnessTracker.Data;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;

namespace FitnessTracker.Services.Create
{
    public class PrimaryMuscleExerciseConnectionCreateService(DataContext context, IReadService<PrimaryMuscleGroupInExercise> readService) : CreateService<PrimaryMuscleGroupInExercise>(context)
    {
        protected override async Task<Exception?> IsEntityValid(PrimaryMuscleGroupInExercise entity)
        {
            if (await readService.Get(x => x.MuscleGroupId == entity.MuscleGroupId && x.ExerciseId == entity.ExerciseId, "none") != null)
                return new EntityAlreadyExistsException();

            return null;
        }
    }
}
