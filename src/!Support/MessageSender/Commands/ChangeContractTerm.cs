namespace ContractManagement.Domain.Aggregates.Contract.Commands;

public record ChangeContractTerm(
    Guid Id,
    string ContractNumber,
    DateTime StartDate,
    DateTime EndDate);