using FitnessTracker.DTOs.Requests.Split;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;

namespace FitnessTracker.Services.ModelServices.SplitService
{
    public partial class SplitService
    {
        public async Task<Guid> CreateComment(Guid splitId, Guid userId, CreateSplitCommentRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Comment))
                throw new InvalidArgumentException("Comment cannot be empty");

            SplitComment mapped = createCommentRequestMapper.Map(request);
            mapped.CreatorId = userId;
            mapped.SplitId = splitId;

            SplitComment newComment = await commentCreateService.Add(mapped);
            return newComment.Id;
        }

        public async Task<Guid> CreateReply(Guid splitId, Guid commentId, Guid userId, CreateSplitCommentRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Comment))
                throw new InvalidArgumentException("Comment cannot be empty");

            SplitComment mapped = createCommentRequestMapper.Map(request);
            mapped.CreatorId = userId;
            mapped.SplitId = splitId;
            mapped.ParentId = commentId;

            SplitComment newComment = await commentCreateService.Add(mapped);
            return newComment.Id;
        }
    }
}
