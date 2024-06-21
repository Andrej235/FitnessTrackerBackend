using FitnessTracker.Data;
using FitnessTracker.Services.Read.ExpressionBased;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Delete
{
    public class DeleteService<T>(DataContext context, IReadSingleService<T> readService) : IDeleteService<T> where T : class
    {
        private readonly DataContext context = context;
        private readonly IReadSingleService<T> readService = readService;

        public async Task Delete(Expression<Func<T, bool>> criteria)
        {
            T entityToDelete = await readService.Get(criteria, "none") ?? throw new NullReferenceException("Entity not found");

            context.Set<T>().Remove(entityToDelete);
            await context.SaveChangesAsync();
        }
    }
}
