using FitnessTracker.Data;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Read.ExpressionBased;

namespace FitnessTracker.Services.Create
{
    public class UserCreateService(DataContext context, IReadSingleService<User> readSingleService) : CreateService<User>(context)
    {
        private readonly IReadSingleService<User> readSingleService = readSingleService;

        protected override async Task<Exception?> IsEntityValid(User entity)
        {
            if (await readSingleService.Get(eq => eq.Email == entity.Email, "none") != null)
                return new EntityAlreadyExistsException();

            return null;
        }
    }
}
