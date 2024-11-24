namespace FitnessTracker.Models
{
    public class UserSettings
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

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
