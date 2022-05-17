namespace ContractManagement.Domain.Aggregates.ContractAggregate.DomainEvents;

public record ContractCancelled(string ContractNumber, string reason) : Event
{
    public static ContractCancelled CreateFrom(CancelContract command) =>
        new ContractCancelled(command.ContractNumber, command.Reason);
}