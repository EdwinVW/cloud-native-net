namespace ContractManagement.Domain.Aggregates.Account.DomainEvents;

public record PaymentAdded(string AccountNumber, decimal Amount) : Event
{
    public static PaymentAdded CeateFrom(AddPayment command) =>
        new PaymentAdded(command.AccountNumber, command.Amount);
}