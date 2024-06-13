using Microsoft.EntityFrameworkCore;
using FitnessTracker.Data;
using FitnessTracker.Models;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Read
{
    public class UserReadService(ExerciseContext context) : AbstractReadService<User>(context)
    {
        protected override Expression<Func<User, bool>> TranslateKeyValueToExpression(string key, string value) => u => false;
    }
}
