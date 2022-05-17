namespace ProductManagement.Application.IntegrationEvents;

public record ProductRegistered(
    string ProductNumber,
    string Description) : Event;
