namespace ContractManagement.Application.IntegrationEvents;

using DomainEvents = ContractManagement.Domain.Aggregates.Contract.DomainEvents;

public record ContractAmountChanged(
    string ContractNumber,
    decimal NewAmount) : Event
{
    public static ContractAmountChanged CreateFrom(DomainEvents.ContractAmountChanged domainEvent) =>
        new ContractAmountChanged(domainEvent.ContractNumber, domainEvent.NewAmount);
}