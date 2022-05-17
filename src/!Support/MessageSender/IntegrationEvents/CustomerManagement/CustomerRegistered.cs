namespace CustomerManagement.Application.IntegrationEvents;

public record CustomerRegistered(
    Guid EventId,
    string CustomerNumber,
    string FirstName,
    string LastName,
    string Address,
    string Email);
