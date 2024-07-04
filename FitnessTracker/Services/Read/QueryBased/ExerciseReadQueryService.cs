using FitnessTracker.Data;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Read.QueryBased
{
    public class ExerciseReadQueryService(DataContext context) : AbstractQueryBasedReadService<Exercise>(context)
    {
        protected override Expression<Func<Exercise, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (key == "name")
                return e => EF.Functions.Like(e.Name, $"%{value}%");
            else
            {
                if (int.TryParse(value, out int valueId))
                {
                    return key switch
                    {
                        "usesmusclegroup" => e => e.PrimaryMuscleGroups.Any(m => m.Id == valueId) || e.SecondaryMuscleGroups.Any(m => m.Id == valueId),
                        "equipment" => e => e.Equipment.Any(eq => eq.Id == valueId),
                        _ => throw new NotSupportedException($"Invalid key in search query. Entered key: {key}"),
                    };
                }

                if (Guid.TryParse(value, out Guid valueGuid))
                {
                    return key switch
                    {
                        "favoritedby" => e => e.Favorites.Any(f => f.Id == valueGuid),
                        _ => throw new NotSupportedException($"Invalid key in search query. Entered key: {key}"),
                    };
                }

                throw new NotSupportedException($"Invalid search query value. Entered value: {value}");
            }
        }
    }
}
