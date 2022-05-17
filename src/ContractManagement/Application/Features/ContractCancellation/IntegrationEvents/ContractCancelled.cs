namespace ContractManagement.Application.IntegrationEvents;

using DomainEvents = ContractManagement.Domain.Aggregates.ContractAggregate.DomainEvents;

public record ContractCancelled(string ContractNumber) : Event
{
    public static ContractCancelled CreateFrom(DomainEvents.ContractCancelled domainEvent) =>
        new ContractCancelled(domainEvent.ContractNumber);
}