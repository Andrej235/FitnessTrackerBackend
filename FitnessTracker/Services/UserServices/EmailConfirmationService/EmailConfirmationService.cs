using FitnessTracker.Models;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Update;
using FitnessTracker.Utilities;

namespace FitnessTracker.Services.UserServices.EmailConfirmationService
{
    public class EmailConfirmationService(IReadSingleService<User> userReadService,
                                          IUpdateService<User> userUpdateService,
                                          IReadSingleService<EmailConfirmation> tokenReadService,
                                          IExecuteDeleteService<EmailConfirmation> executeDeleteService) : IEmailConfirmationService
    {
        private readonly IReadSingleService<User> userReadService = userReadService;
        private readonly IReadSingleService<EmailConfirmation> tokenReadService = tokenReadService;
        private readonly IUpdateService<User> userUpdateService = userUpdateService;
        private readonly IExecuteDeleteService<EmailConfirmation> executeDeleteService = executeDeleteService;

        public async Task<bool> ConfirmEmail(Guid userId, Guid confirmationCode)
        {
            try
            {
                _ = await tokenReadService.Get(x => x.UserId == userId && x.Id == confirmationCode) ?? throw new Exception("Token not found");
                await executeDeleteService.Delete(x => x.UserId == userId);

                User user = await userReadService.Get(x => x.Id == userId) ?? throw new Exception("User not found");
                user.EmailConfirmed = true;
                user.Role = Role.User;
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
