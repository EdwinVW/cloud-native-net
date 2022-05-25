namespace ContractManagement.Application.IntegrationEvents;

using ContractManagement.Domain.Aggregates.Contract.Enums;

public record ContractRegistered(
    Guid EventId,
    string ContractNumber,
    string CustomerNumber,
    string ProductNumber,
    decimal Amount,
    DateTime StartDate,
    DateTime EndDate,
    PaymentPeriod PaymentPeriod);
