using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Update;
using FitnessTracker.Utilities;

namespace FitnessTracker.Services.ModelServices.UserService.ResetPasswordService
{
    public class ResetPasswordService(IReadSingleService<User> userReadService,
                                      IUpdateSingleService<User> userUpdateService,
                                      IReadSingleService<EmailConfirmation> tokenReadService,
                                      IDeleteService<EmailConfirmation> deleteService) : IResetPasswordService
    {
        private readonly IReadSingleService<User> userReadService = userReadService;
        private readonly IUpdateSingleService<User> userUpdateService = userUpdateService;
        private readonly IReadSingleService<EmailConfirmation> tokenReadService = tokenReadService;
        private readonly IDeleteService<EmailConfirmation> deleteService = deleteService;

        public async Task ResetPassword(Guid userId, Guid confirmationCode, string newPassword)
        {
            _ = await tokenReadService.Get(x => x.UserId == userId && x.Id == confirmationCode) ?? throw new NotFoundException("Token not found");
            await deleteService.Delete(x => x.UserId == userId);

            User user = await userReadService.Get(x => x.Id == userId) ?? throw new NotFoundException("User not found");
            user.PasswordHash = newPassword.ToHash(user.Salt);
            await userUpdateService.Update(user);
        }
    }
}
