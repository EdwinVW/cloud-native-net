namespace Infrastructure.Common.Messaging.MassTransit;

public class GrpcOptions
{
    public string Host { get; set; } = "127.0.0.1";
    public int Port { get; set; } = 19796;
    public IEnumerable<string> Servers { get; set; } = new List<string>();
}
