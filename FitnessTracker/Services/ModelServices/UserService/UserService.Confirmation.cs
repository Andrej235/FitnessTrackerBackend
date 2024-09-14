using FitnessTracker.Exceptions;
using FitnessTracker.Utilities;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public partial class UserService
    {
        public async Task ResendConfirmationEmail(Guid userId)
        {
            var user = await readSingleSelectedService.Get(
                x => new
                {
                    x.Email,
                    x.Role
                },
                x => x.Id == userId)
                ?? throw new UnauthorizedException();

            if (user.Role != Role.Unverified)
                throw new BadRequestException("User is already verified");

            await emailConfirmationSender.SendEmailConfirmation(user.Email, userId);
        }

        public Task ConfirmEmail(Guid code, Guid userId)
        {
            if (userId == default)
                throw new UnauthorizedException();

            if (code == default)
                throw new BadRequestException("Invalid code");

            return emailConfirmationService.ConfirmEmail(userId, code);
        }
    }
}
