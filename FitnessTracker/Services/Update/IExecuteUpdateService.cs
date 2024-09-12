using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Update
{
    public interface IExecuteUpdateService<T> where T : class
    {
        /// <summary>
        /// Updates the entities which match the given criteria and sets the given property calls in the database
        /// </summary>
        /// <param name="updateCriteria">The criteria to update</param>
        /// <param name="setPropertyCalls">The property calls to set in the database</param>
        /// <returns></returns>
        Task Execute(Expression<Func<T, bool>> updateCriteria, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls);
    }
}
