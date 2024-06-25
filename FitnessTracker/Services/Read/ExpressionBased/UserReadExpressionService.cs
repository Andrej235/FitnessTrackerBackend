﻿using FitnessTracker.Data;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Services.Read.ExpressionBased
{
    public class UserReadExpressionService(DataContext context) : ReadExpressionService<User>(context)
    {
        protected override IQueryable<User> GetIncluded(string? includeString)
        {
            if (includeString is null)
                return base.GetIncluded(includeString);

            if (includeString == "detailed")
            {
                return context.Users
                    .Include(x => x.Followers)
                    .Include(x => x.Following)
                    .Include(x => x.CompletedWorkouts)
                    .Include(x => x.CurrentSplit!)
                    .ThenInclude(x => x.Workouts)
                    .ThenInclude(x => x.Workout);
            }

            return base.GetIncluded(includeString);
        }
    }
}
