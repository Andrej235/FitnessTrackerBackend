namespace FitnessTracker.Exceptions
{
    [Serializable]
    public class FailedToCreateEntityException : Exception
    {
        public FailedToCreateEntityException() { }
        public FailedToCreateEntityException(string message) : base(message) { }
        public FailedToCreateEntityException(string message, Exception inner) : base(message, inner) { }
    }
}
