using FitnessTracker.Data;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Services.Read.ExpressionBased
{
    public class SplitReadExpressionService(DataContext context) : ReadExpressionService<Split>(context)
    {
        protected override IQueryable<Split> GetIncluded(string? includeString) => includeString is null
                ? base.GetIncluded(includeString)
                : includeString == "detailed"
                ? context.Splits
                    .Include(x => x.Creator)
                    .Include(x => x.Workouts)
                    .ThenInclude(x => x.Workout)
                    .ThenInclude(x => x.Creator)
                    .Include(x => x.Likes)
                    .Include(x => x.Favorites)
                    .Include(x => x.Comments)
                : includeString.Contains("workouts")
                ? base.GetIncluded(includeString.Replace("workouts", ""))
                    .Include(x => x.Workouts)
                    .ThenInclude(x => x.Workout)
                : base.GetIncluded(includeString);
    }
}
