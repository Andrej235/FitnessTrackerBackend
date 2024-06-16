namespace FitnessTracker.Services.UserServices.EmailConfirmationSender
{
    public interface IEmailConfirmationSender
    {
        Task SendEmailConfirmation(string email, Guid userId);
    }
}
