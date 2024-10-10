using FitnessTracker.DTOs.Responses.Split;
using FitnessTracker.Services.Read;

namespace FitnessTracker.Services.ModelServices.SplitService
{
    public partial class SplitService
    {
        public async Task<IEnumerable<SimpleSplitCommentResponseDTO>> GetComments(Guid splitId, Guid? userId, int? offset, int? limit)
        {
            if (userId == default)
                userId = null;

            var comments = await commentReadSelectedRangeService.Get(
                x => new
                {
                    Comment = x,
                    IsLiked = userId != null && x.Likes.Any(x => x.Id == userId),
                    LikeCount = x.Likes.Count,
                    RepliesCount = x.Likes.Count,
                },
                x => x.SplitId == splitId && x.ParentId == null,
                offset,
                limit ?? 10,
                x => x.Include(x => x.Creator));

            IEnumerable<SimpleSplitCommentResponseDTO> mapped = comments.Select(x =>
            {
                SimpleSplitCommentResponseDTO mapped = simpleCommentResponseMapper.Map(x.Comment);
                mapped.IsLiked = x.IsLiked;
                mapped.LikeCount = x.LikeCount;
                mapped.ReplyCount = x.RepliesCount;
                mapped.IsCreator = userId != null && x.Comment.CreatorId == userId;
                return mapped;
            });

            return mapped;
        }

        public async Task<IEnumerable<SimpleSplitCommentResponseDTO>> GetReplies(Guid splitId, Guid commentId, Guid? userId, int? offset, int? limit)
        {
            if (userId == default)
                userId = null;

            var comments = await commentReadSelectedRangeService.Get(
                x => new
                {
                    Comment = x,
                    IsLiked = userId != null && x.Likes.Any(x => x.Id == userId),
                    LikeCount = x.Likes.Count,
                    RepliesCount = x.Likes.Count,
                },
                x => x.SplitId == splitId && x.ParentId == commentId,
                offset,
                limit ?? 10,
                x => x.Include(x => x.Creator));

            IEnumerable<SimpleSplitCommentResponseDTO> mapped = comments.Select(x =>
            {
                SimpleSplitCommentResponseDTO mapped = simpleCommentResponseMapper.Map(x.Comment);
                mapped.IsLiked = x.IsLiked;
                mapped.LikeCount = x.LikeCount;
                mapped.ReplyCount = x.RepliesCount;
                mapped.IsCreator = userId != null && x.Comment.CreatorId == userId;
                return mapped;
            });

            return mapped;
        }
    }
}
