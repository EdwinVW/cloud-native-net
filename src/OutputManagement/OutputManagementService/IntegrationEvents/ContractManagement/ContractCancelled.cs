namespace ContractManagement.Application.IntegrationEvents;

public record ContractCancelled(
    Guid EventId, 
    string ContractNumber);