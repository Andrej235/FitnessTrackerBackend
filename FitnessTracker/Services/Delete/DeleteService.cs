using FitnessTracker.Data;
using FitnessTracker.Services.Read;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Delete
{
    public class DeleteService<T>(DataContext context, IReadSingleService<T> readService) : IDeleteService<T> where T : class
    {
        private readonly DataContext context = context;
        private readonly IReadSingleService<T> readService = readService;

        public async Task Delete(Expression<Func<T, bool>> criteria)
        {
            T entityToDelete = await readService.Get(criteria) ?? throw new NullReferenceException("Entity not found");

            _ = context.Set<T>().Remove(entityToDelete);
            _ = await context.SaveChangesAsync();
        }
    }
}
