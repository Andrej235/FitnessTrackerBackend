using FitnessTracker.Exceptions;

namespace FitnessTracker.Services.ModelServices.SplitService
{
    public partial class SplitService
    {
        public Task CreateFavorite(Guid splitId, Guid userId)
        {
            if (splitId == default)
                throw new InvalidArgumentException($"{nameof(splitId)} cannot be empty");

            return favoriteCreateService.Add(new()
            {
                UserId = userId,
                SplitId = splitId,
                FavoritedAt = DateTime.Now,
            });
        }

        public Task DeleteFavorite(Guid splitId, Guid userId)
        {
            if (splitId == default)
                throw new InvalidArgumentException($"{nameof(splitId)} cannot be empty");

            return favoriteDeleteService.Delete(x => x.UserId == userId && x.SplitId == splitId);
        }
    }
}
