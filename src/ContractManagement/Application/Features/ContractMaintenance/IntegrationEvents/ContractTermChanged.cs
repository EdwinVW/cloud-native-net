namespace ContractManagement.Application.IntegrationEvents;

using DomainEvents = ContractManagement.Domain.Aggregates.Contract.DomainEvents;

public record ContractTermChanged(
    string ContractNumber,
    DateTime StartDate,
    DateTime EndDate) : Event
{
    public static ContractTermChanged CreateFrom(DomainEvents.ContractTermChanged domainEvent) =>
        new ContractTermChanged(domainEvent.ContractNumber, domainEvent.StartDate, domainEvent.EndDate);
}