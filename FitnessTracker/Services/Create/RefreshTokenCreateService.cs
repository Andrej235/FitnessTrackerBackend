using FitnessTracker.Models;
using ProjectGym.Data;
using ProjectGym.Services.Create;

namespace FitnessTracker.Services.Create
{
    public class RefreshTokenCreateService(ExerciseContext context) : ICreateService<RefreshToken>
    {
        private readonly ExerciseContext context = context;

        public async Task<object?> Add(RefreshToken toAdd)
        {
            await context.AddAsync(toAdd);
            await context.SaveChangesAsync();
            return toAdd.Token;
        }
    }
}
