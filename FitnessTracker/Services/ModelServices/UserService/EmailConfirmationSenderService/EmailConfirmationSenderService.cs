﻿using FitnessTracker.Models;
using FitnessTracker.Services.Create;
using FitnessTracker.Services.EmailSender;

namespace FitnessTracker.Services.ModelServices.UserService.EmailConfirmationSenderService
{
    public class EmailConfirmationSenderService(ICreateService<EmailConfirmation> createService,
                                          IEmailSenderService emailSender) : IEmailConfirmationSenderService
    {
        private readonly ICreateService<EmailConfirmation> createService = createService;
        private readonly IEmailSenderService emailSender = emailSender;

        private const string WEB_APP_URL = "http://localhost:5173";
        public async Task SendEmailConfirmation(string email, Guid userId)
        {
            EmailConfirmation mapped = new()
            {
                UserId = userId
            };

            EmailConfirmation newEmailConfirmationToken = await createService.Add(mapped);
            emailSender.SendEmail(new Emails.Message([email], "Email confirmation", $"Please confirm your email by clicking <a href=\"{WEB_APP_URL}/email-verification/{newEmailConfirmationToken.Id}\">here</a><br />Or by entering the following code into settings <b>{newEmailConfirmationToken.Id}</b>"));
            return;
        }
    }
}
