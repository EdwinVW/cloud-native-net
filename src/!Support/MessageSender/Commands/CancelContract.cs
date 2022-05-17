namespace ContractManagement.Domain.Aggregates.ContractAggregate.Commands;

public record CancelContract(
    Guid Id,
    string ContractNumber,
    string Reason);