using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Services.Read;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Read
{
    public class RefreshTokenReadService(ExerciseContext context) : IReadService<RefreshToken>
    {
        private readonly ExerciseContext context = context;

        public Task<RefreshToken?> Get(Expression<Func<RefreshToken, bool>> criteria, string? include = "all") => context.RefreshTokens.Include(x => x.User).FirstOrDefaultAsync(criteria);

        public Task<RefreshToken?> Get(object id, string? include = "all") => Get(x => x.Token == (Guid)id, "all");

        public Task<List<RefreshToken>> Get(Expression<Func<RefreshToken, bool>> criteria, int? offset = 0, int? limit = -1, string? include = "all")
        {
            var tokens = context.RefreshTokens.Where(criteria).Skip(offset ?? 0);
            if (limit != null && limit >= 0)
                tokens = tokens.Take(limit ?? 0);

            return tokens.Any() ? Task.FromResult(tokens.ToList()) : Task.FromResult<List<RefreshToken>>([]);
        }

        public Task<List<RefreshToken>> Get(string? query, int? offset = 0, int? limit = -1, string? include = "all") => Task.FromResult<List<RefreshToken>>([]);
    }
}
