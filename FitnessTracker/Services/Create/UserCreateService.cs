using FitnessTracker.Data;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;

namespace FitnessTracker.Services.Create
{
    public class UserCreateService(ExerciseContext context, IReadService<User> readService) : CreateService<User>(context)
    {
        protected override async Task<Exception?> IsEntityValid(User entity)
        {
            if (await readService.Get(eq => eq.Email == entity.Email, "none") != null)
                return new EntityAlreadyExistsException();

            return null;
        }
    }
}
