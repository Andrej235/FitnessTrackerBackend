using FitnessTracker.Data;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;

namespace FitnessTracker.Services.Create
{
    public class MuscleGroupCreateService(DataContext context, IReadService<MuscleGroup> readService) : CreateService<MuscleGroup>(context)
    {
        protected override async Task<Exception?> IsEntityValid(MuscleGroup entity)
        {
            if (await readService.Get(x => x.Name.ToLower().Trim() == entity.Name.ToLower().Trim(), "none") != null)
                return new EntityAlreadyExistsException();

            return null;
        }
    }
}
