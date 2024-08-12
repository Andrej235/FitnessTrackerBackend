using FitnessTracker.Data;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Services.Read.ExpressionBased
{
    public class FavoriteWorkoutReadExpressionService(DataContext context) : ReadExpressionService<FavoriteWorkout>(context)
    {
        protected override IQueryable<FavoriteWorkout> GetIncluded(string? includeString) => (
                includeString is null
                    ? base.GetIncluded(includeString)
                    : includeString.Contains("workout")
                    ? base.GetIncluded(includeString.Replace("workout", ""))
                        .Include(x => x.Workout)
                        .ThenInclude(x => x.Creator)
                    : base.GetIncluded(includeString)
            ).OrderByDescending(x => x.FavoritedAt);
    }
}
