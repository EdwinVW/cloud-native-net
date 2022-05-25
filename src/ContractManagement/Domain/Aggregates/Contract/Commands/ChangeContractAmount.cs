namespace ContractManagement.Domain.Aggregates.ContractAggregate.Commands;

public record ChangeContractAmount(
    string ContractNumber,
    decimal NewAmount) : Command;