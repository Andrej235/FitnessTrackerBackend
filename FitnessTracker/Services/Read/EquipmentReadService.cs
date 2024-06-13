using Microsoft.EntityFrameworkCore;
using FitnessTracker.Data;
using FitnessTracker.Models;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Read
{
    public class EquipmentReadService(DataContext context) : AbstractReadService<Equipment>(context)
    {
        protected override Expression<Func<Equipment, bool>> TranslateKeyValueToExpression(string key, string value)
        {
            if (key == "name")
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new NullReferenceException("Value in a search query cannot be null or empty.");

                return eq => EF.Functions.Like(eq.Name, $"%{value}%");
            }

            if (key == "exercise" && int.TryParse(value, out var id))
                return x => x.UsedInExercises.Any(x => x.Id == id);

            throw new NotSupportedException($"Invalid key in search query. Entered key: {key}");
        }
    }
}
