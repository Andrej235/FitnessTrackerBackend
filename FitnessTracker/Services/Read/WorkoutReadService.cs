using Microsoft.EntityFrameworkCore;
using FitnessTracker.Data;
using FitnessTracker.Models;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Read
{
    public class WorkoutReadService(DataContext context) : AbstractReadService<Workout>(context)
    {
        protected override IQueryable<Workout> GetIncluded(string? includeString)
        {
            if (includeString is null)
                return base.GetIncluded(includeString);

            if (includeString.Contains("detailed"))
            {
                return context.Workouts
                    .Include(x => x.Creator)
                    .Include(x => x.Sets)
                    .ThenInclude(x => x.Exercise)
                    .Include(x => x.Likes)
                    .Include(x => x.Favorites)
                    .Include(x => x.Comments);
            }

            return base.GetIncluded(includeString);
        }

        protected override Expression<Func<Workout, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (int.TryParse(value, out int valueId))
            {
                return key switch
                {
                    "equipment" => x => x.Sets.Any(x => x.Exercise.Equipment.Any(x => x.Id == valueId)),
                    "primarymusclegroup" => x => x.Sets.Any(x => x.Exercise.PrimaryMuscleGroups.Any(x => x.Id == valueId)),
                    "secondarymusclegroup" => x => x.Sets.Any(x => x.Exercise.SecondaryMuscleGroups.Any(x => x.Id == valueId)),
                    "primarymuscle" => x => x.Sets.Any(x => x.Exercise.PrimaryMuscles.Any(x => x.Id == valueId)),
                    "secondarymuscle" => x => x.Sets.Any(x => x.Exercise.SecondaryMuscles.Any(x => x.Id == valueId)),
                    _ => throw new NotSupportedException($"Invalid key in search query. Entered key: {key}")
                };
            }

            if (value.Contains(','))
            {
                var values = value.Replace(" ", "").Split(',');
                List<int> valueIds = [];
                foreach (var id in values)
                {
                    if (int.TryParse(id, out int newId))
                        valueIds.Add(newId);
                }

                return key switch
                {
                    "primarymusclegroup" => x => x.Sets.Any(x => x.Exercise.PrimaryMuscleGroups.Any(m => valueIds.Contains(m.Id))),
                    "secondarymusclegroup" => x => x.Sets.Any(x => x.Exercise.SecondaryMuscleGroups.Any(m => valueIds.Contains(m.Id))),
                    "primarymuscle" => x => x.Sets.Any(x => x.Exercise.PrimaryMuscles.Any(m => valueIds.Contains(m.Id))),
                    "secondarymuscle" => x => x.Sets.Any(x => x.Exercise.SecondaryMuscles.Any(m => valueIds.Contains(m.Id))),
                    "equipment" => x => x.Sets.Any(x => x.Exercise.Equipment.Any(eq => valueIds.Contains(eq.Id))),
                    _ => throw new NotSupportedException($"Invalid key in search query. Entered key: {key}"),
                };
            }

            throw new NotSupportedException($"Invalid key in search query. Entered key: {key}");
        }
    }
}
