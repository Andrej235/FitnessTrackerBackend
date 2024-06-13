using FitnessTracker.Data;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;

namespace FitnessTracker.Services.Create
{
    public class EquipmentExerciseUsageCreateService(ExerciseContext context, IReadService<EquipmentUsage> readService) : CreateService<EquipmentUsage>(context)
    {
        protected override async Task<Exception?> IsEntityValid(EquipmentUsage entity)
        {
            try
            {
                if (await readService.Get(x => x.EquipmentId == entity.EquipmentId && x.ExerciseId == entity.ExerciseId, "none") != null)
                    return new EntityAlreadyExistsException();

                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
