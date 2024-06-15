namespace FitnessTracker.Emails
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
