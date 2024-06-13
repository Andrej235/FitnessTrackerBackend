using Microsoft.EntityFrameworkCore;
using FitnessTracker.Data;
using FitnessTracker.Models;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Read
{
    public class ExerciseReadService(DataContext context) : AbstractReadService<Exercise>(context)
    {
        protected override Expression<Func<Exercise, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (key == "name")
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new NullReferenceException("Value in a search query cannot be null or empty.");

                return e => EF.Functions.Like(e.Name, $"%{value}%");
            }
            else
            {
                if (int.TryParse(value, out int valueId))
                {
                    return key switch
                    {
                        "id" => e => e.Id == valueId,
                        "primarymusclegroup" => e => e.PrimaryMuscleGroups.Any(m => m.Id == valueId),
                        "secondarymusclegroup" => e => e.SecondaryMuscleGroups.Any(m => m.Id == valueId),
                        "primarymuscle" => e => e.PrimaryMuscles.Any(m => m.Id == valueId),
                        "secondarymuscle" => e => e.SecondaryMuscles.Any(m => m.Id == valueId),
                        "equipment" => e => e.Equipment.Any(eq => eq.Id == valueId),
                        _ => throw new NotSupportedException($"Invalid key in search query. Entered key: {key}"),
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
                        "id" => e => valueIds.Contains(e.Id),
                        "primarymusclegroup" => e => e.PrimaryMuscleGroups.Any(m => valueIds.Contains(m.Id)),
                        "secondarymusclegroup" => e => e.SecondaryMuscleGroups.Any(m => valueIds.Contains(m.Id)),
                        "primarymuscle" => e => e.PrimaryMuscles.Any(m => valueIds.Contains(m.Id)),
                        "secondarymuscle" => e => e.SecondaryMuscles.Any(m => valueIds.Contains(m.Id)),
                        "equipment" => e => e.Equipment.Any(eq => valueIds.Contains(eq.Id)),
                        _ => throw new NotSupportedException($"Invalid key in search query. Entered key: {key}"),
                    };
                }

                throw new NotSupportedException($"Invalid search query value. Entered value: {value}");
            }
        }
    }
}
