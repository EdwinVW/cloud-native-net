namespace ContractManagement.Application.IntegrationEvents;

public record ContractAmountChanged(
    Guid EventId,
    string ContractNumber,
    decimal NewAmount);