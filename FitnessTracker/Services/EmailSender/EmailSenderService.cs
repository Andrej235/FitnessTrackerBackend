using FitnessTracker.Emails;
using FitnessTracker.Exceptions;
using MailKit.Net.Smtp;
using MimeKit;

namespace FitnessTracker.Services.EmailSender
{
    public class EmailSenderService(EmailConfiguration emailConfig, SmtpClient client) : IEmailSenderService
    {
        private readonly EmailConfiguration emailConfig = emailConfig;
        private readonly SmtpClient client = client;

        public void SendEmail(Message message)
        {
            try
            {
                MimeMessage emailMessage = CreateEmailMessage(message);

                Send(emailMessage);
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Failed to send email", ex);
            }
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            MimeMessage emailMessage = new();
            emailMessage.From.Add(new MailboxAddress("Fitness tracker", emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message.Content
            };
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            client.Connect(emailConfig.SmtpServer, emailConfig.Port, true);
            _ = client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(emailConfig.UserName, emailConfig.Password);
            _ = client.Send(mailMessage);
        }
    }
}
