using FitnessTracker.DTOs.Responses.User;

namespace FitnessTracker.DTOs.Responses.Split
{
    public class SimpleSplitCommentResponseDTO
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = null!;
        public SimpleUserResponseDTO Creator { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int LikeCount { get; set; }
        public bool IsLiked { get; set; }
        public bool IsCreator { get; set; }
        public int ReplyCount { get; set; }
        public Guid SplitId { get; set; }
    }
}
