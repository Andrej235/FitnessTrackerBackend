namespace FitnessTracker.Services.ModelServices.UserService.EmailConfirmationService
{
    public interface IEmailConfirmationService
    {
        Task ConfirmEmail(Guid userId, Guid confirmationCode);
    }
}
