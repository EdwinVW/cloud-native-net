namespace Application.Common.Exceptions;

[Serializable]
public class ConsistencyException : Exception
{
    private List<string> _details = new List<string>();

    public IEnumerable<string> Details => _details;

    public ConsistencyException()
    {
    }

    public ConsistencyException(string message) : base(message)
    {
    }

    public ConsistencyException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected ConsistencyException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public void AddDetail(string detailMessage)
    {
        _details.Add(detailMessage);
    }
}