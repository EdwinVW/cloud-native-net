namespace ContractManagement.Domain.Aggregates.ContractAggregate.Commands;

public record ChangeContractAmount(
    Guid Id,
    string ContractNumber,
    decimal NewAmount);