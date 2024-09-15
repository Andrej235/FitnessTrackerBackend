using FitnessTracker.Data;
using FitnessTracker.Exceptions;
using FitnessTracker.Utilities;

namespace FitnessTracker.Services.Create
{
    public class CreateService<T>(DataContext context) : ICreateService<T>, ICreateRangeService<T> where T : class
    {
        public async Task<T> Add(T toAdd)
        {
            try
            {
                _ = await context.Set<T>().AddAsync(toAdd);
                _ = await context.SaveChangesAsync();

                return toAdd;
            }
            catch (Exception ex)
            {
                ex.LogError();
                throw new FailedToCreateEntityException("Failed to create entity", ex);
            }
        }

        public async Task Add(IEnumerable<T> toAdd)
        {
            try
            {
                List<T> toAddList = toAdd.ToList();
                await context.Set<T>().AddRangeAsync(toAddList);
                _ = await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ex.LogError();
                throw new FailedToCreateEntityException("Failed to create entity", ex);
            }
        }
    }
}
