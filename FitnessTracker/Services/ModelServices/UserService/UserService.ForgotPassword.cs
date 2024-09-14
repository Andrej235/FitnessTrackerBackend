using FitnessTracker.DTOs.Requests.User;

namespace FitnessTracker.Services.ModelServices.UserService
{
    public partial class UserService
    {
        public Task SendForgotPasswordEmail(SendForgotPasswordEmailRequestDTO request) => throw new NotImplementedException();
        public Task ConfirmForgotPasswordEmail(Guid code, ResetPasswordUserRequestDTO request) => throw new NotImplementedException();
    }
}
