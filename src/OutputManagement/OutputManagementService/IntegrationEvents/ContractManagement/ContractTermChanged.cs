namespace ContractManagement.Application.IntegrationEvents;

public record ContractTermChanged(
    Guid EventId,
    string ContractNumber,
    DateTime StartDate,
    DateTime EndDate);