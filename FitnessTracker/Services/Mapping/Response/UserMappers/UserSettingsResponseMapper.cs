using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.UserMappers
{
    public class UserSettingsResponseMapper : IResponseMapper<UserSettings, UserSettingsResponseDTO>
    {
        public UserSettingsResponseDTO Map(UserSettings from) => new()
        {
            PublicFollowing = from.PublicFollowing,
            PublicCompletedWorkouts = from.PublicCompletedWorkouts,
            PublicStreak = from.PublicStreak,
            PublicCurrentSplit = from.PublicCurrentSplit,
            PublicLikedWorkouts = from.PublicLikedWorkouts,
            PublicFavoriteWorkouts = from.PublicFavoriteWorkouts,
            PublicLikedSplits = from.PublicLikedSplits,
            PublicFavoriteSplits = from.PublicFavoriteSplits,
        };
    }
}
