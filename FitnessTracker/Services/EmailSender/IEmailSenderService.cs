using FitnessTracker.Emails;

namespace FitnessTracker.Services.EmailSender
{
    public interface IEmailSenderService
    {
        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="message">Message to send to specified recipients</param>
        /// <exception cref="Exceptions.BadRequestException" />
        void SendEmail(Message message);
    }
}
