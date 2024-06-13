using Microsoft.EntityFrameworkCore;
using FitnessTracker.Data;
using FitnessTracker.Models;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Read
{
    public class MuscleReadService(DataContext context) : AbstractReadService<Muscle>(context)
    {
        protected override Expression<Func<Muscle, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (key == "name")
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new NullReferenceException("Value in a search query cannot be null or empty.");

                return x => EF.Functions.Like(x.Name, $"%{value}%");
            }

            if (int.TryParse(value, out var id))
            {
                return key switch
                {
                    "musclegroup" => x => x.MuscleGroupId == id,
                    "primaryin" => x => x.PrimaryInExercises.Any(x => x.Id == id),
                    "primary" => x => x.PrimaryInExercises.Any(x => x.Id == id),
                    "secondaryin" => x => x.SecondaryInExercises.Any(x => x.Id == id),
                    "secondary" => x => x.SecondaryInExercises.Any(x => x.Id == id),
                    _ => throw new NotSupportedException($"Invalid key in search query. Entered key: {key}")
                };
            }
            throw new NotSupportedException($"Invalid value in search query. Entered value '{value}' for key '{key}'");
        }
    }
}
