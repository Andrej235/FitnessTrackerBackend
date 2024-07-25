using FitnessTracker.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Read.Count
{
    public class CountService<T>(DataContext context) : ICountService<T> where T : class
    {
        public async Task<int> Count(Expression<Func<T, bool>> criteria) => await context.Set<T>().Where(criteria).CountAsync();
    }
}
