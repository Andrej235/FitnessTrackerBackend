namespace FitnessTracker.Services.UserServices.ResetPasswordService
{
    public interface IResetPasswordService
    {
        Task<bool> ResetPassword(Guid userId, Guid confirmationCode, string newPassword);
    }
}
