namespace FitnessTracker.DTOs.Requests.User
{
    public class UpdateUserSettingsRequestDTO
    {
        public bool PublicFollowing { get; set; }
        public bool PublicStreak { get; set; }
        public bool PublicCurrentSplit { get; set; }

        public bool PublicCreatedWorkouts { get; set; }
        public bool PublicLikedWorkouts { get; set; }
        public bool PublicFavoriteWorkouts { get; set; }

        public bool PublicCreatedSplits { get; set; }
        public bool PublicLikedSplits { get; set; }
        public bool PublicFavoriteSplits { get; set; }
    }
}
