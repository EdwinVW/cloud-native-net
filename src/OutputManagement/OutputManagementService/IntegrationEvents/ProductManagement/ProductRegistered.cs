namespace ProductManagement.Application.IntegrationEvents;

public record ProductRegistered(
    Guid EventId,
    string ProductNumber,
    string Description);
