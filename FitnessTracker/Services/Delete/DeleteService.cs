using FitnessTracker.Data;
using FitnessTracker.Services.Read;
using FitnessTracker.Utilities;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Delete
{
    public class DeleteService<T>(DataContext context, IReadService<T> readService) : IDeleteService<T> where T : class
    {
        public async Task Delete(object id)
        {
            T entityToDelete = await readService.Get(id, "none") ?? throw new NullReferenceException("Entity not found");

            context.Set<T>().Remove(entityToDelete);
            await context.SaveChangesAsync();
        }

        public async Task DeleteFirst(Expression<Func<T, bool>> criteria)
        {
            T entityToDelete = await readService.Get(criteria, "none") ?? throw new NullReferenceException("Entity not found");

            context.Set<T>().Remove(entityToDelete);
            await context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAll(Expression<Func<T, bool>> criteria)
        {
            List<T> entitiesToDelete = await readService.Get(criteria, 0, -1, "none");

            if (entitiesToDelete.Count != 0)
            {
                try
                {
                    context.Set<T>().RemoveRange(entitiesToDelete);
                    await context.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    LogDebugger.LogError(ex);
                    return false;
                }
            }
            return false;
        }
    }
}
