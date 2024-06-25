using FitnessTracker.Data;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Services.Read.ExpressionBased
{
    public class CompletedWorkoutReadExpressionService(DataContext context) : ReadExpressionService<CompletedWorkout>(context)
    {
        protected override IQueryable<CompletedWorkout> GetIncluded(string? includeString)
        {
            if(includeString is null)
                return base.GetIncluded(includeString);

            if (includeString == "detailed")
            {
                return context.CompletedWorkouts
                    .Include(x => x.Split)
                    .ThenInclude(x => x.Workouts);
            }

            return base.GetIncluded(includeString);
        }
    }
}
