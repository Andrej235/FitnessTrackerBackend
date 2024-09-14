using FitnessTracker.DTOs.Requests.Split;

namespace FitnessTracker.Services.ModelServices.SplitService
{
    public partial class SplitService
    {
        public Task CreateComment(Guid splitId, Guid userId, CreateSplitCommentRequestDTO request) => throw new NotImplementedException();
        public Task CreateReply(Guid splitId, Guid commentId, Guid userId, CreateSplitCommentRequestDTO request) => throw new NotImplementedException();
    }
}
