namespace CustomerManagement.Application.IntegrationEvents;

public record CustomerRegistered(
    string CustomerNumber,
    string FirstName,
    string LastName,
    string Address,
    string Email) : Event;
