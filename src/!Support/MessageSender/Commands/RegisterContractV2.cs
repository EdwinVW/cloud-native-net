using ContractManagement.Domain.Aggregates.Contract.Enums;

namespace ContractManagement.Domain.Aggregates.Contract.Commands;

public record RegisterContractV2(
    Guid Id,
    string ContractNumber,
    string CustomerNumber,
    string ProductNumber,
    decimal Amount,
    DateTime StartDate,
    DateTime EndDate,
    PaymentPeriod PaymentPeriod);