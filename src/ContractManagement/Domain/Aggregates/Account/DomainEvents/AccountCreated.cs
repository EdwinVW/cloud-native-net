namespace ContractManagement.Domain.Aggregates.Account.DomainEvents;

public record AccountCreated(string AccountNumber) : Event
{
    public static AccountCreated CreateFrom(CreateAccount command) =>
        new AccountCreated(command.AccountNumber);
}