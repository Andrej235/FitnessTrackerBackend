using FitnessTracker.Data;
using FitnessTracker.Exceptions;
using FitnessTracker.Utilities;

namespace FitnessTracker.Services.Create
{
    public class CreateService<T>(DataContext context) : ICreateService<T>, ICreateRangeService<T> where T : class
    {
        public async Task<object?> Add(T toAdd)
        {
            try
            {
                Exception? validationError = await IsEntityValid(toAdd);
                if (validationError != null)
                    throw validationError;

                _ = await context.Set<T>().AddAsync(toAdd);
                _ = await context.SaveChangesAsync();

                object? id = toAdd.GetType().GetProperty("Id")?.GetValue(toAdd);
                return id ?? throw new PropertyNotFoundException("Entity doesn't have an Id property");
            }
            catch (Exception ex)
            {
                ex.LogError();
                return null;
            }
        }

        public async Task<bool> Add(IEnumerable<T> toAdd)
        {
            try
            {
                List<T> toAddList = toAdd.ToList();
                foreach (T? item in toAddList)
                {
                    if (await IsEntityValid(item) != null)
                        throw new EntityAlreadyExistsException();
                }

                await context.Set<T>().AddRangeAsync(toAddList);
                _ = await context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                ex.LogError();
                return false;
            }
        }

        protected virtual Task<Exception?> IsEntityValid(T entity) => Task.FromResult(default(Exception));
    }
}
