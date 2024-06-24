using FitnessTracker.Data;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Services.Read.ExpressionBased
{
    public class SplitReadExpressionService(DataContext context) : ReadExpressionService<Split>(context)
    {
        protected override IQueryable<Split> GetIncluded(string? includeString)
        {
            if (includeString is null)
                return base.GetIncluded(includeString);

            if (includeString.Contains("detailed"))
            {
                return context.Splits
                    .Include(x => x.Creator)
                    .Include(x => x.Workouts)
                    .ThenInclude(x => x.Workout)
                    .ThenInclude(x => x.Creator)
                    .Include(x => x.Likes)
                    .Include(x => x.Favorites)
                    .Include(x => x.Comments);
            }

            return base.GetIncluded(includeString);
        }
    }
}
