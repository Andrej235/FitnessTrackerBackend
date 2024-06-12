using ProjectGym.Data;
using ProjectGym.Exceptions;
using ProjectGym.Models;
using ProjectGym.Services.Read;

namespace ProjectGym.Services.Create
{
    public class PrimaryMuscleExerciseConnectionCreateService(ExerciseContext context, IReadService<PrimaryMuscleGroupInExercise> readService) : CreateService<PrimaryMuscleGroupInExercise>(context)
    {
        protected override async Task<Exception?> IsEntityValid(PrimaryMuscleGroupInExercise entity)
        {
            if (await readService.Get(x => x.MuscleGroupId == entity.MuscleGroupId && x.ExerciseId == entity.ExerciseId, "none") != null)
                return new EntityAlreadyExistsException();

            return null;
        }
    }
}
