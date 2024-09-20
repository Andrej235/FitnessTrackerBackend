namespace FitnessTracker.Services.ModelServices.UserService.ResetPasswordSenderService
{
    public interface IResetPasswordEmailSenderService
    {
        public Task SendEmail(string email, Guid UserId);
    }
}
