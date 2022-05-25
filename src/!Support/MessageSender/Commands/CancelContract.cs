namespace ContractManagement.Domain.Aggregates.Contract.Commands;

public record CancelContract(
    Guid Id,
    string ContractNumber,
    string Reason);