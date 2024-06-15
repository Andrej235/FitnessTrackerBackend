using FitnessTracker.Utilities;
using MailKit.Net.Smtp;
using MimeKit;

namespace FitnessTracker.Emails
{
    public class EmailSender(EmailConfiguration emailConfig) : IEmailSender
    {
        private readonly EmailConfiguration emailConfig = emailConfig;

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
                Text = $"<div bgcolor=\"red\"><h1>{message.Content}</h1></div>"
            };
            return emailMessage;
        }
        private void Send(MimeMessage mailMessage)
        {
            var client = new SmtpClient();
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

            client.Disconnect(true);
            client.Dispose();
        }
    }
}
