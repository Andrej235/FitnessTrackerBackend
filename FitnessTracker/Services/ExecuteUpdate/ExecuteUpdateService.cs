using FitnessTracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace FitnessTracker.Services.ExecuteUpdate
{
    public class ExecuteUpdateService<T>(DataContext context) : IExecuteUpdateService<T> where T : class
    {
        private readonly DataContext context = context;

        public async Task Execute(Expression<Func<T, bool>> updateCriteria, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls) =>
            await context.Set<T>().Where(updateCriteria).ExecuteUpdateAsync(setPropertyCalls);
    }
}
