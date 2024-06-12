using ProjectGym.Data;
using ProjectGym.Exceptions;
using ProjectGym.Utilities;

namespace ProjectGym.Services.Create
{
    public class CreateService<T>(ExerciseContext context) : ICreateService<T> where T : class
    {
        public async Task<object?> Add(T toAdd)
        {
            try
            {
                var validationError = await IsEntityValid(toAdd);
                if (validationError != null)
                    throw validationError;

                await context.Set<T>().AddAsync(toAdd);
                await context.SaveChangesAsync();

                var id = toAdd.GetType().GetProperty("Id")?.GetValue(toAdd);
                return id ?? throw new PropertyNotFoundException("Entity doesn't have an Id property");
            }
            catch (Exception ex)
            {
                ex.LogError();
                return null;
            }
        }

        protected virtual Task<Exception?> IsEntityValid(T entity) => Task.FromResult(default(Exception));
    }
}
