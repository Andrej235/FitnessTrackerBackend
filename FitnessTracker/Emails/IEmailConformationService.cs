namespace FitnessTracker.Emails
{
    public interface IEmailConformationService
    {
        Task SendEmailConfirmation(string email, Guid userId);
        Task<bool> ConfirmEmail(Guid userId, Guid confirmationCode);
    }
}
