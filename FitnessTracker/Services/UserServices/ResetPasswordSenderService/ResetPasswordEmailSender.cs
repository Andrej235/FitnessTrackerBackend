using FitnessTracker.Emails;
using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.EmailSender;

namespace FitnessTracker.Services.UserServices.ResetPasswordSenderService
{
    public class ResetPasswordEmailSender(ICreateService<EmailConfirmation> createService,
                                          IEmailSenderService emailSender) : IResetPasswordEmailSenderService
    {
        private readonly ICreateService<EmailConfirmation> createService = createService;
        private readonly IEmailSenderService emailSender = emailSender;

        private const string WEB_APP_URL = "http://localhost:5173";

        public async Task SendEmail(string email, Guid userId)
        {
            EmailConfirmation mapped = new()
            {
                UserId = userId
            };

            EmailConfirmation newEmailConfirmationToken = await createService.Add(mapped);
            emailSender.SendEmail(new Message([email], "Email confirmation", $"Reset your password by clicking <a href=\"{WEB_APP_URL}/reset-password/{newEmailConfirmationToken.Id}\">here</a>"));
            return;
        }
    }
}
