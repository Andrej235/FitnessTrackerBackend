using FitnessTracker.Models;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Update;
using FitnessTracker.Utilities;

namespace FitnessTracker.Services.UserServices.EmailConfirmationService
{
    public class EmailConfirmationService(IReadService<User> userReadService,
                                          IUpdateService<User> userUpdateService,
                                          IDeleteRangeService<EmailConfirmation> deleteRangeService) : IEmailConfirmationService
    {
        private readonly IReadService<Models.User> userReadService = userReadService;
        private readonly IUpdateService<Models.User> userUpdateService = userUpdateService;
        private readonly IDeleteRangeService<EmailConfirmation> deleteRangeService = deleteRangeService;

        public async Task<bool> ConfirmEmail(Guid userId, Guid confirmationCode)
        {
            try
            {
                bool deletedAny = await deleteRangeService.Delete(x => x.UserId == userId);
                if (!deletedAny)
                    throw new Exception("User does not have any email confirmation codes");

                var user = await userReadService.Get(x => x.Id == userId, "none") ?? throw new Exception("User not found");
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
