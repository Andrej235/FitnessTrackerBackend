using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.Delete;
using FitnessTracker.Services.Read;
using FitnessTracker.Services.Update;

namespace FitnessTracker.Emails
{
    public class EmailConformationService(ICreateService<EmailConformation> createService,
                                          IReadService<User> userReadService,
                                          IUpdateService<User> userUpdateService,
                                          IDeleteService<EmailConformation> deleteService,
                                          IEmailSender emailSender) : IEmailConformationService
    {
        private readonly ICreateService<EmailConformation> createService = createService;
        private readonly IReadService<User> userReadService = userReadService;
        private readonly IUpdateService<User> userUpdateService = userUpdateService;
        private readonly IDeleteService<EmailConformation> deleteService = deleteService;
        private readonly IEmailSender emailSender = emailSender;

        public async Task<bool> ConfirmEmail(Guid userId, Guid confirmationCode)
        {
            try
            {
                await deleteService.DeleteFirst(x => x.UserId == userId && x.Id == confirmationCode);
                var user = await userReadService.Get(x => x.Id == userId, "none");
                user!.EmailConfirmed = true;
                await userUpdateService.Update(user);
                await deleteService.DeleteAll(x => x.UserId == userId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task SendEmailConformation(string email, Guid userId)
        {
            EmailConformation emailConformation = new()
            {
                UserId = userId
            };

            var emailConformationId = await createService.Add(emailConformation);
            if (emailConformationId == default || emailConformationId is not Guid conformationCode)
                throw new Exception("Failed to create email conformation");

            emailSender.SendEmail(new Message([email], "Email conformation", conformationCode.ToString()));
            return;
        }
    }
}
