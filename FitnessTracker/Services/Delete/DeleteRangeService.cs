using FitnessTracker.Data;
using FitnessTracker.Services.Read;
using FitnessTracker.Utilities;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Delete
{
    public class DeleteRangeService<T>(DataContext context, IReadService<T> readService) : IDeleteRangeService<T> where T : class
    {
        private readonly DataContext context = context;
        private readonly IReadService<T> readService = readService;

        public async Task<bool> Delete(Expression<Func<T, bool>> criteria)
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
