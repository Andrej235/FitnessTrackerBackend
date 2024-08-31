
using FitnessTracker.Models;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Read.Full;
using FitnessTracker.Services.Update;
using FitnessTracker.Utilities;

namespace FitnessTracker.Services.UserServices.ResetPasswordService
{
    public class ResetPasswordService(IFullReadService<User> userReadService,
                                      IUpdateService<Models.User> userUpdateService,
                                      IDeleteRangeService<EmailConfirmation> deleteRangeService) : IResetPasswordService
    {
        private readonly IFullReadService<User> userReadService = userReadService;
        private readonly IUpdateService<User> userUpdateService = userUpdateService;
        private readonly IDeleteRangeService<EmailConfirmation> deleteRangeService = deleteRangeService;

        public async Task<bool> ResetPassword(Guid userId, Guid confirmationCode, string newPassword)
        {
            try
            {
                bool deletedAny = await deleteRangeService.Delete(x => x.UserId == userId);
                if (!deletedAny)
                    throw new Exception("User does not have any email confirmation codes");

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
