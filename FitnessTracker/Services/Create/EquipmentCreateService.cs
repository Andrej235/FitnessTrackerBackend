using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Exceptions;
using ProjectGym.Models;
using ProjectGym.Services.Read;

namespace ProjectGym.Services.Create
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
