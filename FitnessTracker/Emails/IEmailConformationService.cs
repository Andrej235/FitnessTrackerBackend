namespace FitnessTracker.Emails
{
    public interface IEmailConformationService
    {
        Task SendEmailConformation(string email, Guid userId);
        Task<bool> ConfirmEmail(Guid userId, Guid confirmationCode);
    }
}
