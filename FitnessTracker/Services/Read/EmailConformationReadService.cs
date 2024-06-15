using FitnessTracker.Data;
using FitnessTracker.Models;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Read
{
    public class EmailConformationReadService(DataContext context) : AbstractReadService<EmailConformation>(context)
    {
        protected override Expression<Func<EmailConformation, bool>> TranslateKeyValueToExpression(string key, string value) => x => true;
    }
}
