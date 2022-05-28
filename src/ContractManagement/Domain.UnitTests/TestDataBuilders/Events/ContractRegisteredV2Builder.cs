namespace Domain.UnitTests.TestDataBuilders.Events;

public class ContractRegisteredV2Builder
{
    public static ContractRegisteredV2 Build(string aggregateId)
    {
        var contractNumber = aggregateId;
        var customerNumber = "C72856";
        var productNumber = "FAC-00241";
        decimal amount = 100000;
        DateTime startDate = new DateTime(2022, 4, 24, 13, 47, 26);
        DateTime endDate = new DateTime(2027, 4, 24, 18, 33, 5);
        PaymentPeriod paymentPeriod = PaymentPeriod.Monthly;
 
        return new ContractRegisteredV2(
            contractNumber,
            customerNumber,
            productNumber,
            amount,
            startDate,
            endDate, 
            paymentPeriod);
    }
}