namespace FitnessTracker.DTOs.Responses.User
{
    public class UserSettingsResponseDTO
    {
        public bool PublicFollowing { get; set; }
        public bool PublicCompletedWorkouts { get; set; }
        public bool PublicStreak { get; set; }
        public bool PublicCurrentSplit { get; set; }

        public bool PublicLikedWorkouts { get; set; }
        public bool PublicFavoriteWorkouts { get; set; }
        public bool PublicLikedSplits { get; set; }
        public bool PublicFavoriteSplits { get; set; }
    }
}
