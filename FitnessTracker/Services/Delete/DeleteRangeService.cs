using FitnessTracker.Data;
using FitnessTracker.Services.Read;
using FitnessTracker.Utilities;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Delete
{
    public class DeleteRangeService<T>(DataContext context, IReadRangeService<T> readService) : IDeleteRangeService<T> where T : class
    {
        private readonly DataContext context = context;
        private readonly IReadRangeService<T> readService = readService;

        public async Task<bool> Delete(Expression<Func<T, bool>> criteria)
        {
            IEnumerable<T> entitiesToDelete = await readService.Get(criteria);

            if (entitiesToDelete.Any())
            {
                try
                {
                    context.Set<T>().RemoveRange(entitiesToDelete);
                    _ = await context.SaveChangesAsync();
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
