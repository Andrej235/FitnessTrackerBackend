using FitnessTracker.DTOs.Requests.User;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public partial class UserService
    {
        public async Task UpdateSettings(Guid userId, UpdateUserSettingsRequestDTO request)
        {
            UserSettings? settings = await settingsReadSingleService.Get(x => x.UserId == userId, x => x.AsNoTracking()) ?? throw new UnauthorizedException();

            settings.PublicFollowing = request.PublicFollowing;
            settings.PublicStreak = request.PublicStreak;
            settings.PublicCurrentSplit = request.PublicCurrentSplit;
            settings.PublicLikedWorkouts = request.PublicLikedWorkouts;
            settings.PublicFavoriteWorkouts = request.PublicFavoriteWorkouts;
            settings.PublicLikedSplits = request.PublicLikedSplits;
            settings.PublicFavoriteSplits = request.PublicFavoriteSplits;
            await settingsUpdateService.Update(settings);
        }
    }
}
