namespace ContractManagement.Domain.Aggregates.ContractAggregate.Commands;

public record ChangeContractTerm(
    string ContractNumber,
    DateTime StartDate,
    DateTime EndDate) : Command;