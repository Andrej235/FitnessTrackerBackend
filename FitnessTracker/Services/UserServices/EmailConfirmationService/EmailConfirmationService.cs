using FitnessTracker.Models;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Update;
using FitnessTracker.Utilities;

namespace FitnessTracker.Services.UserServices.EmailConfirmationService
{
    public class EmailConfirmationService(IReadService<User> userReadService,
                                          IUpdateService<User> userUpdateService,
                                          IDeleteService<EmailConfirmation> deleteService) : IEmailConfirmationService
    {
        private readonly IReadService<Models.User> userReadService = userReadService;
        private readonly IUpdateService<Models.User> userUpdateService = userUpdateService;
        private readonly IDeleteService<EmailConfirmation> deleteService = deleteService;

        public async Task<bool> ConfirmEmail(Guid userId, Guid confirmationCode)
        {
            try
            {
                await deleteService.DeleteFirst(x => x.UserId == userId && x.Id == confirmationCode);
                var user = await userReadService.Get(x => x.Id == userId, "none") ?? throw new Exception("User not found");

                user.EmailConfirmed = true;
                user.Role = Role.User;
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
