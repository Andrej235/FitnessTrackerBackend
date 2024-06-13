using Microsoft.EntityFrameworkCore;
using FitnessTracker.Data;
using FitnessTracker.Models;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Read
{
    public class MuscleGroupReadService(ExerciseContext context) : AbstractReadService<MuscleGroup>(context)
    {
        protected override Expression<Func<MuscleGroup, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (key == "name")
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new NullReferenceException("Value in a search query cannot be null or empty.");

                return m => EF.Functions.Like(m.Name, $"%{value}%");
            }

            if (int.TryParse(value, out int id))
            {
                return key switch
                {
                    "primaryin" => x => x.PrimaryInExercises.Any(x => x.Id == id),
                    "primary" => x => x.PrimaryInExercises.Any(x => x.Id == id),
                    "secondaryin" => x => x.SecondaryInExercises.Any(x => x.Id == id),
                    "secondary" => x => x.SecondaryInExercises.Any(x => x.Id == id),
                    _ => throw new NotSupportedException($"Invalid key in search query. Entered key: {key}")
                };
            }

            throw new NotSupportedException($"Invalid key in search query. Entered key: {key}");
        }
    }
}
