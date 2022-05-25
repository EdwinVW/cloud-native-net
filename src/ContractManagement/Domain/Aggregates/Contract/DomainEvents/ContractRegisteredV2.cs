namespace ContractManagement.Domain.Aggregates.Contract.DomainEvents;

public record ContractRegisteredV2(
    string ContractNumber,
    string CustomerNumber,
    string ProductNumber,
    decimal Amount,
    DateTime StartDate,
    DateTime EndDate,
    PaymentPeriod PaymentPeriod) : Event
{
    public static ContractRegisteredV2 CreateFrom(RegisterContract command) =>
    new ContractRegisteredV2(
        command.ContractNumber,
        command.CustomerNumber,
        command.ProductNumber,
        command.Amount,
        command.StartDate,
        command.EndDate,
        PaymentPeriod.Monthly);  

    public static ContractRegisteredV2 CreateFrom(RegisterContractV2 command) =>
        new ContractRegisteredV2(
            command.ContractNumber,
            command.CustomerNumber,
            command.ProductNumber,
            command.Amount,
            command.StartDate,
            command.EndDate,
            command.PaymentPeriod);

    public static ContractRegisteredV2 CreateFrom(ContractRegistered domainEvent) =>
    new ContractRegisteredV2(
        domainEvent.ContractNumber,
        domainEvent.CustomerNumber,
        domainEvent.ProductNumber,
        domainEvent.Amount,
        domainEvent.StartDate,
        domainEvent.EndDate,
        PaymentPeriod.Monthly);                  
}