namespace ContractManagement.Domain.Aggregates.ContractAggregate.DomainEvents;

public record ContractAmountChanged(
    string ContractNumber,
    decimal NewAmount) : Event
{
    public static ContractAmountChanged CreateFrom(ChangeContractAmount command) =>
        new ContractAmountChanged(
            command.ContractNumber,
            command.NewAmount);
}