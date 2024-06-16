namespace FitnessTracker.Emails.Sender
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
