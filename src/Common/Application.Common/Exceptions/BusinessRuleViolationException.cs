namespace Application.Common.Exceptions;

[Serializable]
public class BusinessRuleViolationException : Exception
{
    private List<string> _violations = new List<string>();

    public IEnumerable<string> Violations => _violations;

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

    public void AddViolation(string violationMessage)
    {
        _violations.Add(violationMessage);
    }
}