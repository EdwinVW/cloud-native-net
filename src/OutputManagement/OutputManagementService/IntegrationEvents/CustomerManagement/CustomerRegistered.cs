namespace CustomerManagement.Application.IntegrationEvents;

public record CustomerRegistered(
    Guid EventId,
    string CustomerNumber,
    string Email);
