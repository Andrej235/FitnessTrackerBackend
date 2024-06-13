using FitnessTracker.Models;
using FitnessTracker.Data;
using FitnessTracker.Services.Create;

namespace FitnessTracker.Services.Create
{
    public class RefreshTokenCreateService(DataContext context) : ICreateService<RefreshToken>
    {
        private readonly DataContext context = context;

        public async Task<object?> Add(RefreshToken toAdd)
        {
            await context.AddAsync(toAdd);
            await context.SaveChangesAsync();
            return toAdd.Token;
        }
    }
}
