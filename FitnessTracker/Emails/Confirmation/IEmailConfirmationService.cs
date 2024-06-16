namespace FitnessTracker.Emails.Confirmation
{
    public interface IEmailConfirmationService
    {
        Task SendEmailConfirmation(string email, Guid userId);
        Task<bool> ConfirmEmail(Guid userId, Guid confirmationCode);
    }
}
