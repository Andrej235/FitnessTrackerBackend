namespace FitnessTracker.Services.UserServices.EmailConfirmationSenderService
{
    public interface IEmailConfirmationSenderService
    {
        Task SendEmailConfirmation(string email, Guid userId);
    }
}
