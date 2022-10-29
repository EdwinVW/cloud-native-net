namespace ContractManagement.Domain.Aggregates.Account.Commands;

public record AddPayment(string AccountNumber, decimal Amount) : Command;
