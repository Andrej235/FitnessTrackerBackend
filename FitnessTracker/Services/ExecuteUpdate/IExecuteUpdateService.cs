using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace FitnessTracker.Services.ExecuteUpdate
{
    public interface IExecuteUpdateService<T> where T : class
    {
        Task Execute(Expression<Func<T, bool>> updateCriteria, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls);
    }
}
