using FitnessTracker.Exceptions;

namespace FitnessTracker.Services.ModelServices.SplitService
{
    public partial class SplitService
    {
        public Task CreateLike(Guid splitId, Guid userId)
        {
            if (splitId == default)
                throw new InvalidArgumentException($"{nameof(splitId)} cannot be empty");

            return likeCreateService.Add(new()
            {
                UserId = userId,
                SplitId = splitId,
            });
        }

        public Task DeleteLike(Guid splitId, Guid userId)
        {
            if (splitId == default)
                throw new InvalidArgumentException($"{nameof(splitId)} cannot be empty");

            return likeDeleteService.Delete(x => x.UserId == userId && x.SplitId == splitId);
        }
    }
}
