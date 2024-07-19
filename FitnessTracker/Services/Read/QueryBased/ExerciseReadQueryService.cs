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
                return int.TryParse(value, out int valueId)
                    ? key switch
                    {
                        "usesmusclegroup" => e => e.PrimaryMuscleGroups.Any(m => m.Id == valueId) || e.SecondaryMuscleGroups.Any(m => m.Id == valueId),
                        "usesequipment" => e => e.Equipment.Any(eq => eq.Id == valueId),
                        _ => throw new NotSupportedException($"Invalid key in search query. Entered key: {key}"),
                    }
                    : Guid.TryParse(value, out Guid valueGuid)
                    ? key switch
                    {
                        "favoritedby" => e => e.Favorites.Any(f => f.Id == valueGuid),
                        _ => throw new NotSupportedException($"Invalid key in search query. Entered key: {key}"),
                    }
                    : throw new NotSupportedException($"Invalid search query value. Entered value: {value}");
            }
        }
    }
}
