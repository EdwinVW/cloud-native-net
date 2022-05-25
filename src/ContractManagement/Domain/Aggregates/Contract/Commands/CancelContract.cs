namespace ContractManagement.Domain.Aggregates.ContractAggregate.Commands;

public record CancelContract(
    string ContractNumber,
    string Reason) : Command;