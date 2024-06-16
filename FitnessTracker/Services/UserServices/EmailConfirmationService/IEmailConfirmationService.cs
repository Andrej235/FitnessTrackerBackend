namespace FitnessTracker.Services.UserServices.EmailConfirmationService
{
    public interface IEmailConfirmationService
    {
        Task<bool> ConfirmEmail(Guid userId, Guid confirmationCode);
    }
}
