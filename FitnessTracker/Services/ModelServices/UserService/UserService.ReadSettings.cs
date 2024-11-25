using FitnessTracker.DTOs.Responses.User;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Services.Read;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public partial class UserService
    {
        public async Task<UserSettingsResponseDTO> GetSettings(string username)
        {
            Guid userId = (await readSingleSelectedService.Get(x => new { x.Id }, x => x.Username == username) ?? throw new UnauthorizedException()).Id;

            UserSettings? settings = await settingsReadSingleService.Get(x => x.UserId == userId, x => x.AsNoTracking()) ?? throw new UnauthorizedException();
            return settingsResponseMapper.Map(settings);
        }
    }
}
