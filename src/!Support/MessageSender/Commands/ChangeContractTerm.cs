namespace ContractManagement.Domain.Aggregates.ContractAggregate.Commands;

public record ChangeContractTerm(
    Guid Id,
    string ContractNumber,
    DateTime StartDate,
    DateTime EndDate);