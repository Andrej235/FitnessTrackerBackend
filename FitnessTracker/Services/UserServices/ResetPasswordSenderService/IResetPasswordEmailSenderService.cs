namespace FitnessTracker.Services.UserServices.ResetPasswordSenderService
{
    public interface IResetPasswordEmailSenderService
    {
        public Task SendEmail(string email, Guid UserId);
    }
}
