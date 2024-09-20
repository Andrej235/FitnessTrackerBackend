namespace FitnessTracker.Services.ModelServices.UserService.ResetPasswordService
{
    public interface IResetPasswordService
    {
        Task ResetPassword(Guid userId, Guid confirmationCode, string newPassword);
    }
}
