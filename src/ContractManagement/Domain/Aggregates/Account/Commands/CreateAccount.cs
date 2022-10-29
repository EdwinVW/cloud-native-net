namespace ContractManagement.Domain.Aggregates.Account.Commands;

public record CreateAccount(string AccountNumber) : Command;
