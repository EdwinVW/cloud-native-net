namespace Application.Common.Exceptions;

[Serializable]
public class BusinessRuleViolationException : Exception
{
    private List<string> _details = new List<string>();

    public IEnumerable<string> Details => _details;

    public BusinessRuleViolationException()
    {
    }

    public BusinessRuleViolationException(string message) : base(message)
    {
    }

    public BusinessRuleViolationException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected BusinessRuleViolationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public void AddDetail(string detailMessage)
    {
        _details.Add(detailMessage);
    }
}