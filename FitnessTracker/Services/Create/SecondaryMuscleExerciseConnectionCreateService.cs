using ProjectGym.Data;
using ProjectGym.Exceptions;
using ProjectGym.Models;
using ProjectGym.Services.Read;

namespace ProjectGym.Services.Create
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
