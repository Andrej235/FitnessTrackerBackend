using ProjectGym.Data;
using ProjectGym.Exceptions;
using ProjectGym.Models;
using ProjectGym.Services.Read;

namespace ProjectGym.Services.Create
{
    public class MuscleGroupCreateService(ExerciseContext context, IReadService<MuscleGroup> readService) : CreateService<MuscleGroup>(context)
    {
        protected override async Task<Exception?> IsEntityValid(MuscleGroup entity)
        {
            if (await readService.Get(x => x.Name.ToLower().Trim() == entity.Name.ToLower().Trim(), "none") != null)
                return new EntityAlreadyExistsException();

            return null;
        }
    }
}
