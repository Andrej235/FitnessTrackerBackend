namespace FitnessTracker.Services.UserServices.ResetPasswordSender
{
    public interface IResetPasswordEmailSender
    {
        public Task SendEmail(string email, Guid UserId);
    }
}
