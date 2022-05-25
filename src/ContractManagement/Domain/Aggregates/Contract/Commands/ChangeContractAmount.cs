namespace ContractManagement.Domain.Aggregates.Contract.Commands;

public record ChangeContractAmount(
    string ContractNumber,
    decimal NewAmount) : Command;