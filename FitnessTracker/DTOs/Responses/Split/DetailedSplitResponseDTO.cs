namespace FitnessTracker.DTOs.Responses.Split
{
    public class DetailedSplitResponseDTO : SimpleSplitResponseDTO
    {
        public string Description { get; set; } = null!;

        public IEnumerable<SimpleSplitWorkoutResponseDTO> Workouts { get; set; } = [];

        public int LikeCount { get; set; }
        public int FavoriteCount { get; set; }
        public int CommentCount { get; set; }
        public bool IsLiked { get; set; }
        public bool IsFavorited { get; set; }
    }
}
