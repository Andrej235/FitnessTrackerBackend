using FitnessTracker.Exceptions;
using FitnessTracker.Models;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Update;
using FitnessTracker.Utilities;

namespace FitnessTracker.Services.UserServices.EmailConfirmationService
{
    public class EmailConfirmationService(IReadSingleService<User> userReadService,
                                          IUpdateSingleService<User> userUpdateService,
                                          IReadSingleService<EmailConfirmation> tokenReadService,
                                          IDeleteService<EmailConfirmation> deleteService) : IEmailConfirmationService
    {
        private readonly IReadSingleService<User> userReadService = userReadService;
        private readonly IReadSingleService<EmailConfirmation> tokenReadService = tokenReadService;
        private readonly IUpdateSingleService<User> userUpdateService = userUpdateService;
        private readonly IDeleteService<EmailConfirmation> deleteService = deleteService;

        public async Task ConfirmEmail(Guid userId, Guid confirmationCode)
        {
            try
            {
                _ = await tokenReadService.Get(x => x.UserId == userId && x.Id == confirmationCode) ?? throw new NotFoundException("Confirmation code not found");
                await deleteService.Delete(x => x.UserId == userId);

                User user = await userReadService.Get(x => x.Id == userId) ?? throw new NotFoundException("User not found");
                user.EmailConfirmed = true;
                user.Role = Role.User;
                await userUpdateService.Update(user);
            }
            catch (Exception ex)
            {
                ex.LogError();
                throw new BadRequestException("Failed to confirm email", ex);
            }
        }
    }
}
