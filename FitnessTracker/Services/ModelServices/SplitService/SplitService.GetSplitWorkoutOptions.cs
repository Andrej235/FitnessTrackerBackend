using FitnessTracker.DTOs.Responses.Workout;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;
using FitnessTracker.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FitnessTracker.Services.ModelServices.SplitService
{
    public partial class SplitService
    {
        public async Task<IEnumerable<SimpleWorkoutOptionResponseDTO>> GetSplitWorkoutOptions(Guid userId, int? offset, int? limit, string? nameFilter, bool? publicOnly, bool? favoritesOnly, bool? personalOnly)
        {
            List<Expression<Func<Workout, bool>>> filters = [];

            if (nameFilter is not null)
                filters.Add(x => EF.Functions.Like(x.Name, $"%{nameFilter}%"));

            if (publicOnly is true)
                filters.Add(x => x.IsPublic);

            if (favoritesOnly is true)
                filters.Add(x => x.Favorites.Any(x => x.Id == userId));

            if (personalOnly is true)
                filters.Add(x => x.CreatorId == userId);

            var workoutOptions = await workoutReadSelectedRangeService.Get(
                x => new
                {
                    Workout = x,
                    LikeCount = x.Likes.Count,
                },
                filters.Combine(ExpressionExtensions.CombineOperator.AND),
                offset,
                limit ?? 10,
                x => x.Include(x => x.Creator));

            //TODO: Sort the results based on how many criteria they match
            return workoutOptions.Select(x =>
            {
                SimpleWorkoutOptionResponseDTO mapped = simpleWorkoutOptionResponseMapper.Map(x.Workout);
                mapped.LikeCount = x.LikeCount;
                return mapped;
            });
        }
    }
}
