namespace ContractManagement.Domain.Aggregates.Contract.Commands;

public record ChangeContractTerm(
    string ContractNumber,
    DateTime StartDate,
    DateTime EndDate) : Command;