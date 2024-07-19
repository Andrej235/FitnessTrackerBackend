using FitnessTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Services.Update
{
    public class UpdateService<T>(DataContext context) : IUpdateService<T> where T : class
    {
        public async Task Update(T updatedEntity)
        {
            try
            {
                if (!await context.Set<T>().ContainsAsync(updatedEntity))
                    throw new NullReferenceException("Entity not found");

                _ = context.Set<T>().Update(updatedEntity);
                _ = await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
