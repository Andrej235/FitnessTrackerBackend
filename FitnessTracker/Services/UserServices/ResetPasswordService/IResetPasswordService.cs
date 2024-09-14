namespace FitnessTracker.Services.UserServices.ResetPasswordService
{
    public interface IResetPasswordService
    {
        Task ResetPassword(Guid userId, Guid confirmationCode, string newPassword);
    }
}
