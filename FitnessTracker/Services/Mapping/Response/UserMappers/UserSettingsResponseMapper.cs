using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Models;

namespace FitnessTracker.Services.Mapping.Response.UserMappers
{
    public class UserSettingsResponseMapper : IResponseMapper<UserSettings, UserSettingsResponseDTO>
    {
        public UserSettingsResponseDTO Map(UserSettings from) => new()
        {
            PublicFollowing = from.PublicFollowing,
            PublicStreak = from.PublicStreak,
            PublicCurrentSplit = from.PublicCurrentSplit,
            PublicCreatedWorkouts = from.PublicCreatedWorkouts,
            PublicLikedWorkouts = from.PublicLikedWorkouts,
            PublicFavoriteWorkouts = from.PublicFavoriteWorkouts,
            PublicCreatedSplits = from.PublicCreatedSplits,
            PublicLikedSplits = from.PublicLikedSplits,
            PublicFavoriteSplits = from.PublicFavoriteSplits,
        };
    }
}
