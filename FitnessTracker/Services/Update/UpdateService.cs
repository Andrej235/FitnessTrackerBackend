using FitnessTracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Update
{
    public class UpdateService<T>(DataContext context) : IUpdateService<T>, IUpdateRangeService<T>, IExecuteUpdateService<T> where T : class
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

        public async Task Update(IEnumerable<T> updatedEntities)
        {
            try
            {
                context.Set<T>().UpdateRange(updatedEntities);
                _ = await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Execute(Expression<Func<T, bool>> updateCriteria, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls) =>
            await context.Set<T>().Where(updateCriteria).ExecuteUpdateAsync(setPropertyCalls);
    }
}
