namespace FitnessTracker.Services.UserServices.EmailConfirmationService
{
    public interface IEmailConfirmationService
    {
        Task ConfirmEmail(Guid userId, Guid confirmationCode);
    }
}
