namespace ContractManagement.Domain.Aggregates.ContractAggregate.Commands;

public record RegisterContractV2(
    string ContractNumber,
    string CustomerNumber,
    string ProductNumber,
    decimal Amount,
    DateTime StartDate,
    DateTime EndDate,
    PaymentPeriod PaymentPeriod) : Command
{
    public static RegisterContractV2 CreateFrom(RegisterContract command) =>
        new RegisterContractV2(
            command.ContractNumber,
            command.CustomerNumber,
            command.ProductNumber,
            command.Amount,
            command.StartDate,
            command.EndDate,
            PaymentPeriod.Monthly);
}