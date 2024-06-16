using FitnessTracker.Data;
using FitnessTracker.Models;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Read
{
    public class EmailConfirmationReadService(DataContext context) : AbstractReadService<EmailConfirmation>(context)
    {
        protected override Expression<Func<EmailConfirmation, bool>> TranslateKeyValueToExpression(string key, string value) => x => true;
    }
}
