using FitnessTracker.Utilities;
using MailKit.Net.Smtp;
using MimeKit;

namespace FitnessTracker.Emails.Sender
{
    public class EmailSender(EmailConfiguration emailConfig, SmtpClient client) : IEmailSender
    {
        private readonly EmailConfiguration emailConfig = emailConfig;
        private readonly SmtpClient client = client;

        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);

            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
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
            try
            {
                client.Connect(emailConfig.SmtpServer, emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(emailConfig.UserName, emailConfig.Password);
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                ex.LogError();
            }
        }
    }
}
