namespace FitnessTracker.Services.ModelServices.UserService.EmailConfirmationSenderService
{
    public interface IEmailConfirmationSenderService
    {
        Task SendEmailConfirmation(string email, Guid userId);
    }
}
