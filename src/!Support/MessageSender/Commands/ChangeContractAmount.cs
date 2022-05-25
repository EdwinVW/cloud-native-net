namespace ContractManagement.Domain.Aggregates.Contract.Commands;

public record ChangeContractAmount(
    Guid Id,
    string ContractNumber,
    decimal NewAmount);