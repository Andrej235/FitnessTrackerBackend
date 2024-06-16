
using FitnessTracker.Models;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Update;
using FitnessTracker.Utilities;

namespace FitnessTracker.Services.UserServices.ResetPasswordService
{
    public class ResetPasswordService(IReadService<Models.User> userReadService,
                                      IUpdateService<Models.User> userUpdateService,
                                      IDeleteService<EmailConfirmation> deleteService) : IResetPasswordService
    {
        public async Task<bool> ResetPassword(Guid userId, Guid confirmationCode, string newPassword)
        {
            try
            {
                await deleteService.DeleteFirst(x => x.UserId == userId && x.Id == confirmationCode);
                var user = await userReadService.Get(x => x.Id == userId, "none") ?? throw new Exception("User not found");
                user.PasswordHash = newPassword.ToHash(user.Salt);

                await userUpdateService.Update(user);
                await deleteService.DeleteAll(x => x.UserId == userId);
                return true;
            }
            catch (Exception ex)
            {
                ex.LogError();
                return false;
            }
        }
    }
}
