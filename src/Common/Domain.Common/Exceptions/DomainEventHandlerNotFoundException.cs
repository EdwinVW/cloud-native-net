namespace Domain.Common.Exceptions
{
    public class DomainEventHandlerNotFoundException : Exception
    {
        public DomainEventHandlerNotFoundException()
        {
        }

        public DomainEventHandlerNotFoundException(string? message) : base(message)
        {
        }

        public DomainEventHandlerNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DomainEventHandlerNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}