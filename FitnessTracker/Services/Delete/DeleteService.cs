using FitnessTracker.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Delete
{
    public class DeleteService<T>(DataContext context) : IDeleteService<T> where T : class
    {
        private readonly DataContext context = context;

        public async Task Delete(Expression<Func<T, bool>> deleteCriteria) => await context.Set<T>().Where(deleteCriteria).ExecuteDeleteAsync();
    }
}
