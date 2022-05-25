namespace ContractManagement.Domain.Aggregates.Contract.Commands;

public record CancelContract(
    string ContractNumber,
    string Reason) : Command;