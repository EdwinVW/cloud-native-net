namespace ContractManagement.Domain.Aggregates.Contract.DomainEvents;

public record ContractCancelled(string ContractNumber, string reason) : Event
{
    public static ContractCancelled CreateFrom(CancelContract command) =>
        new ContractCancelled(command.ContractNumber, command.Reason);
}