using FitnessTracker.DTOs.Responses.Split;

namespace FitnessTracker.Services.ModelServices.SplitService
{
    public partial class SplitService
    {
        public Task<IEnumerable<SimpleSplitCommentResponseDTO>> GetComments(Guid splitId, Guid? userId, int? offset, int? limit) => throw new NotImplementedException();
        public Task<IEnumerable<SimpleSplitCommentResponseDTO>> GetReplies(Guid splitId, Guid commentId, Guid? userId, int? offset, int? limit) => throw new NotImplementedException();
    }
}
