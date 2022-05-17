namespace Domain.Common.Exceptions
{
    public class InvalidValueObjectException : Exception
    {
        public InvalidValueObjectException()
        {
        }

        public InvalidValueObjectException(string? message) : base(message)
        {
        }

        public InvalidValueObjectException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidValueObjectException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}