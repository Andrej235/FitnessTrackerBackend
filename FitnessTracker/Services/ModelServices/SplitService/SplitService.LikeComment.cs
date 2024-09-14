using FitnessTracker.Exceptions;

namespace FitnessTracker.Services.ModelServices.SplitService
{
    public partial class SplitService
    {
        public Task CreateCommentLike(Guid commentId, Guid userId)
        {
            if (commentId == default)
                throw new InvalidArgumentException($"{nameof(commentId)} cannot be empty");

            return commentLikeCreateService.Add(new()
            {
                UserId = userId,
                SplitCommentId = commentId
            });
        }

        public async Task DeleteCommentLike(Guid commentId, Guid userId)
        {
            if (commentId == default)
                throw new InvalidArgumentException($"{nameof(commentId)} cannot be empty");

            await commentLikeDeleteService.Delete(x => x.UserId == userId && x.SplitCommentId == commentId);
        }
    }
}
