using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.EmailSender;

namespace FitnessTracker.Services.UserServices.EmailConfirmationSenderService
{
    public class EmailConfirmationSenderService(ICreateService<EmailConfirmation> createService,
                                          IEmailSenderService emailSender) : IEmailConfirmationSenderService
    {
        private readonly ICreateService<EmailConfirmation> createService = createService;
        private readonly IEmailSenderService emailSender = emailSender;

        private const string WEB_APP_URL = "http://localhost:5173";
        public async Task SendEmailConfirmation(string email, Guid userId)
        {
            EmailConfirmation emailConfirmation = new()
            {
                UserId = userId
            };

            object? emailConfirmationId = await createService.Add(emailConfirmation);
            if (emailConfirmationId == default || emailConfirmationId is not Guid confirmationCode)
                throw new Exception("Failed to create email confirmation");

            emailSender.SendEmail(new Emails.Message([email], "Email confirmation", $"Please confirm your email by clicking <a href=\"{WEB_APP_URL}/email-verification/{confirmationCode}\">here</a>"));
            return;
        }
    }
}
