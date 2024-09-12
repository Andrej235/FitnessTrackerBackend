
using FitnessTracker.Models;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Update;
using FitnessTracker.Utilities;

namespace FitnessTracker.Services.UserServices.ResetPasswordService
{
    public class ResetPasswordService(IReadSingleService<User> userReadService,
                                      IUpdateService<Models.User> userUpdateService,
                                      IReadSingleService<EmailConfirmation> tokenReadService,
                                      IExecuteDeleteService<EmailConfirmation> executeDeleteService) : IResetPasswordService
    {
        private readonly IReadSingleService<User> userReadService = userReadService;
        private readonly IUpdateService<User> userUpdateService = userUpdateService;
        private readonly IReadSingleService<EmailConfirmation> tokenReadService = tokenReadService;
        private readonly IExecuteDeleteService<EmailConfirmation> executeDeleteService = executeDeleteService;

        public async Task<bool> ResetPassword(Guid userId, Guid confirmationCode, string newPassword)
        {
            try
            {
                _ = await tokenReadService.Get(x => x.UserId == userId && x.Id == confirmationCode) ?? throw new Exception("Token not found");
                await executeDeleteService.Delete(x => x.UserId == userId);

                User user = await userReadService.Get(x => x.Id == userId) ?? throw new Exception("User not found");
                user.PasswordHash = newPassword.ToHash(user.Salt);
                await userUpdateService.Update(user);
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
