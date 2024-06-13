using Microsoft.EntityFrameworkCore;
using FitnessTracker.Data;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;

namespace FitnessTracker.Services.Create
{
    public class EquipmentCreateService(ExerciseContext context, IReadService<Equipment> readService) : CreateService<Equipment>(context)
    {
        protected override async Task<Exception?> IsEntityValid(Equipment entity)
        {
            try
            {
                if (await readService.Get(eq => EF.Functions.Like(entity.Name, $"%{entity.Name}%"), "none") != null)
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
