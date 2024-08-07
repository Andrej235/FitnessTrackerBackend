﻿using FitnessTracker.Data;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Services.Read.ExpressionBased
{
    public class CompletedWorkoutReadExpressionService(DataContext context) : ReadExpressionService<CompletedWorkout>(context)
    {
        protected override IQueryable<CompletedWorkout> GetIncluded(string? includeString)
        {
            if (includeString is null)
                return base.GetIncluded(includeString);

            if (includeString == "overview")
            {
                return context.CompletedWorkouts
                    .Include(x => x.Split)
                    .ThenInclude(x => x.Workouts);
            }

            if (includeString == "detailed")
            {
                return context.CompletedWorkouts
                    .Include(x => x.Split)
                    .ThenInclude(x => x.Workouts)
                    .ThenInclude(x => x.Workout)
                    .ThenInclude(x => x.Creator)
                    .Include(x => x.Split)
                    .ThenInclude(x => x.Creator);
            }

            return includeString == "sets,25"
                ? context.CompletedWorkouts
                    .Include(x => x.CompletedSets)
                    .OrderByDescending(x => x.CompletedAt)
                    .Take(25)
                : includeString == "sets,latest"
                ? context.CompletedWorkouts
                    .Include(x => x.CompletedSets)
                    .OrderByDescending(x => x.CompletedAt)
                    .Take(1)
                : base.GetIncluded(includeString);
        }
    }
}
