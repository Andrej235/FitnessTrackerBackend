using FitnessTracker.DTOs.Responses.Set;

namespace FitnessTracker.DTOs.Responses.Workout
{
    public class DetailedWorkoutResponseDTO : BaseWorkoutResponseDTO
    {
        public IEnumerable<DetailedSetResponseDTO> Sets { get; set; } = [];
        public string? Description { get; set; }
        public int LikeCount { get; set; }
        public int FavoriteCount { get; set; }
        public int CommentCount { get; set; }
        public bool IsLiked { get; set; }
        public bool IsFavorited { get; set; }
    }
}
