using FitnessTracker.DTOs.Requests.User;
using FitnessTracker.Exceptions;
using FitnessTracker.Models;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public partial class UserService
    {
        public async Task SendForgotPasswordEmail(SendForgotPasswordEmailRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ArgumentNullException(nameof(request));

            User? user = await readSingleService.Get(x => x.Email == request.Email) ?? throw new UnauthorizedException();
            await passwordResetEmailSender.SendEmail(user.Email, user.Id);
        }

        public async Task ConfirmForgotPasswordEmail(Guid code, ResetPasswordUserRequestDTO request)
        {
            if (request.NewPassword.Length < 8)
                throw new BadRequestException("Password must be at least 8 characters long");

            var user = await readSingleSelectedService.Get(
                x => new
                {
                    x.Id,
                },
                x => x.Email == request.Email)
                ?? throw new UnauthorizedException();

            await passwordResetEmailService.ResetPassword(user.Id, code, request.NewPassword);
        }
    }
}
