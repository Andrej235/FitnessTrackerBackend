using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Services.Read;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Read
{
    public class RoleReadService(ExerciseContext context) : IReadService<Role>
    {
        private readonly ExerciseContext context = context;

        public Task<Role> Get(Expression<Func<Role, bool>> criteria, string? include = "all") => context.Roles.FirstAsync(criteria);

        public async Task<Role> Get(object id, string? include = "all") => await context.Roles.FindAsync(id) ?? throw new NullReferenceException();

        public Task<List<Role>> Get(Expression<Func<Role, bool>> criteria, int? offset = 0, int? limit = -1, string? include = "all")
        {
            var roles = context.Roles.Where(criteria).Skip(offset ?? 0);
            if (limit != null && limit >= 0)
                roles = roles.Take(limit ?? 0);

            return roles.ToListAsync();
        }

        public Task<List<Role>> Get(string? query, int? offset = 0, int? limit = -1, string? include = "all") => Task.FromResult<List<Role>>([]);
    }
}
