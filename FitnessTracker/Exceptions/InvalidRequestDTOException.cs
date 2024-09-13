namespace FitnessTracker.Exceptions
{
    [Serializable]
    public class InvalidRequestDTOException : BadRequestException
    {
        public InvalidRequestDTOException() { }
        public InvalidRequestDTOException(string message) : base(message) { }
        public InvalidRequestDTOException(string message, Exception inner) : base(message, inner) { }
    }
}
