namespace FitnessTracker.Services.ModelServices.SplitService
{
    public partial class SplitService
    {
        public Task CreateCommentLike(Guid commentId, Guid userId) => commentLikeCreateService.Add(new()
        {
            UserId = userId,
            SplitCommentId = commentId
        });

        public Task DeleteCommentLike(Guid commentId, Guid userId) => commentLikeDeleteService.Delete(x => x.UserId == userId && x.SplitCommentId == commentId);
    }
}
