using FitnessTracker.Emails;

namespace FitnessTracker.Services.EmailSender
{
    public interface IEmailSenderService
    {
        void SendEmail(Message message);
    }
}
