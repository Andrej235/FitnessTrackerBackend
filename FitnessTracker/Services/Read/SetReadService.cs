using FitnessTracker.Data;
using FitnessTracker.Models;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Read
{
    public class SetReadService(DataContext context) : AbstractReadService<Set>(context)
    {
        protected override Expression<Func<Set, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (int.TryParse(value, out int valueId))
            {
                return key switch
                {
                    "equipment" => x => x.Exercise.Equipment.Any(x => x.Id == valueId),
                    "primarymusclegroup" => x => x.Exercise.PrimaryMuscleGroups.Any(x => x.Id == valueId),
                    "secondarymusclegroup" => x => x.Exercise.SecondaryMuscleGroups.Any(x => x.Id == valueId),
                    "primarymuscle" => x => x.Exercise.PrimaryMuscles.Any(x => x.Id == valueId),
                    "secondarymuscle" => x => x.Exercise.SecondaryMuscles.Any(x => x.Id == valueId),
                    _ => throw new NotSupportedException($"Invalid key in search query. Entered key: {key}")
                };
            }
            else
            {
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
                        "primarymusclegroup" => x => x.Exercise.PrimaryMuscleGroups.Any(m => valueIds.Contains(m.Id)),
                        "secondarymusclegroup" => x => x.Exercise.SecondaryMuscleGroups.Any(m => valueIds.Contains(m.Id)),
                        "primarymuscle" => x => x.Exercise.PrimaryMuscles.Any(m => valueIds.Contains(m.Id)),
                        "secondarymuscle" => x => x.Exercise.SecondaryMuscles.Any(m => valueIds.Contains(m.Id)),
                        "equipment" => x => x.Exercise.Equipment.Any(eq => valueIds.Contains(eq.Id)),
                        _ => throw new NotSupportedException($"Invalid key in search query. Entered key: {key}"),
                    };
                }
                throw new NotSupportedException($"Invalid key in search query. Entered key: {key}");
            }
        }
    }
}
