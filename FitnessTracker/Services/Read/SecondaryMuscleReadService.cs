using FitnessTracker.Data;
using FitnessTracker.Models;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Read
{
    public class SecondaryMuscleReadService(ExerciseContext context) : AbstractReadService<SecondaryMuscleInExercise>(context)
    {
        protected override Expression<Func<SecondaryMuscleInExercise, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (int.TryParse(value, out int valueId))
            {
                return key switch
                {
                    "muscle" => x => x.MuscleId == valueId,
                    "exercise" => x => x.ExerciseId == valueId,
                    _ => throw new NotSupportedException($"Invalid key in search query. Entered key: {key}"),
                };
            }
            throw new NotSupportedException($"Invalid search query value. Entered value: {value}");
        }
    }
}
