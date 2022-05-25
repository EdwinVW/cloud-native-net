namespace ContractManagement.Domain.Aggregates.Contract.Commands;

public record RegisterContract(
    string ContractNumber,
    string CustomerNumber,
    string ProductNumber,
    decimal Amount,
    DateTime StartDate,
    DateTime EndDate) : Command;