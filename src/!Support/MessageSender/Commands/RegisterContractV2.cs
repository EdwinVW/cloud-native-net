using ContractManagement.Domain.Aggregates.ContractAggregate.Enums;

namespace ContractManagement.Domain.Aggregates.ContractAggregate.Commands;

public record RegisterContractV2(
    Guid Id,
    string ContractNumber,
    string CustomerNumber,
    string ProductNumber,
    decimal Amount,
    DateTime StartDate,
    DateTime EndDate,
    PaymentPeriod PaymentPeriod);