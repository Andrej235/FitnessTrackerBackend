namespace FitnessTracker.Services.ModelServices.SplitService
{
    public partial class SplitService
    {
        public async Task DeleteComment(Guid splitId, Guid commentId, Guid userId)
        {
            await commentDeleteService.Delete(x => x.SplitId == splitId && x.ParentId == commentId);
            await commentDeleteService.Delete(x => x.SplitId == splitId && x.CreatorId == userId && x.Id == commentId);
        }
    }
}
