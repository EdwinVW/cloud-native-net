namespace Infrastructure.Common.Messaging.MassTransit
{
    public class MassTransitOptions
    {
        public bool IsConsumer { get; set; } = false;
        public GrpcOptions? GrpcOptions { get; set; }
        public RabbitMQOptions? RabbitMqOptions { get; set; }
    }
}