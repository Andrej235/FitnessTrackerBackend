using FitnessTracker.Data;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;

namespace FitnessTracker.Services.Create
{
    public class SecondaryMuscleExerciseConnectionCreateService(ExerciseContext context, IReadService<SecondaryMuscleGroupInExercise> readService) : CreateService<SecondaryMuscleGroupInExercise>(context)
    {
        protected override async Task<Exception?> IsEntityValid(SecondaryMuscleGroupInExercise entity)
        {
            if (await readService.Get(x => x.MuscleGroupId == entity.MuscleGroupId && x.ExerciseId == entity.ExerciseId, "none") != null)
                return new EntityAlreadyExistsException();

            return null;
        }
    }
}
