using FitnessTracker.DTOs.Requests.User;
using FitnessTracker.Services.Read;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessTracker.Controllers
{
    public partial class UserController
    {
        [Authorize]
        [HttpPut("me/settings")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateSettings([FromBody] UpdateUserSettingsRequestDTO request)
        {
            if (User.Identity is not ClaimsIdentity claimsIdentity
                || claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value is not string userIdString
                || !Guid.TryParse(userIdString, out Guid userId))
                return Unauthorized();

            Models.UserSettings? settings = await settingsReadSingleService.Get(x => x.UserId == userId, x => x.AsNoTracking());

            if (settings is null)
                return Unauthorized();

            settings.PublicFollowing = request.PublicFollowing;
            settings.PublicCompletedWorkouts = request.PublicCompletedWorkouts;
            settings.PublicStreak = request.PublicStreak;
            settings.PublicCurrentSplit = request.PublicCurrentSplit;
            settings.PublicLikedWorkouts = request.PublicLikedWorkouts;
            settings.PublicFavoriteWorkouts = request.PublicFavoriteWorkouts;
            settings.PublicLikedSplits = request.PublicLikedSplits;
            settings.PublicFavoriteSplits = request.PublicFavoriteSplits;
            await settingsUpdateService.Update(settings);

            return NoContent();
        }
    }
}
