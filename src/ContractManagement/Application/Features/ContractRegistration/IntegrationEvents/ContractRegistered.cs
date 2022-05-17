namespace ContractManagement.Application.IntegrationEvents;

using DomainEvents = ContractManagement.Domain.Aggregates.ContractAggregate.DomainEvents;

public record ContractRegistered(
    string ContractNumber,
    string CustomerNumber,
    string ProductNumber,
    decimal Amount,
    DateTime StartDate,
    DateTime EndDate,
    PaymentPeriod PaymentPeriod) : Event
{
    public static ContractRegistered CreateFrom(DomainEvents.ContractRegisteredV2 domainEvent) =>
        new ContractRegistered(
            ContractNumber: domainEvent.ContractNumber,
            CustomerNumber: domainEvent.CustomerNumber,
            ProductNumber: domainEvent.ProductNumber,
            Amount: domainEvent.Amount,
            StartDate: domainEvent.StartDate,
            EndDate: domainEvent.EndDate,
            PaymentPeriod: domainEvent.PaymentPeriod);
}