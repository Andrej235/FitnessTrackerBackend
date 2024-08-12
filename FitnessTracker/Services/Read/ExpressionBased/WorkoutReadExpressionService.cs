using FitnessTracker.Data;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Services.Read.ExpressionBased
{
    public class WorkoutReadExpressionService(DataContext context) : ReadExpressionService<Workout>(context)
    {
        protected override IQueryable<Workout> GetIncluded(string? includeString)
        {
            if (includeString is null)
            {
                return base.GetIncluded(includeString);
            }

            if (includeString.Contains("sortbynewest"))
                return GetIncluded(includeString.Replace("sortbynewest", "")).OrderByDescending(x => x.CreatedAt);

            if (includeString.Contains("detailed"))
            {
                return context.Workouts
                    .Include(x => x.Creator)
                    .Include(x => x.Sets)
                    .ThenInclude(x => x.Exercise);
            }

            return base.GetIncluded(includeString);
        }
    }
}
