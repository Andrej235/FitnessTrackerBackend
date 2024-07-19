using FitnessTracker.Data;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Services.Read.ExpressionBased
{
    public class WorkoutReadExpressionService(DataContext context) : ReadExpressionService<Workout>(context)
    {
        protected override IQueryable<Workout> GetIncluded(string? includeString) => includeString is null
                ? base.GetIncluded(includeString)
                : includeString.Contains("detailed")
                ? context.Workouts
                    .Include(x => x.Creator)
                    .Include(x => x.Sets)
                    .ThenInclude(x => x.Exercise)
                    .Include(x => x.Likes)
                    .Include(x => x.Favorites)
                    .Include(x => x.Comments)
                : base.GetIncluded(includeString);
    }
}
