namespace Domain.UnitTests.TestDataBuilders.Events;

public class ContractRegisteredBuilder
{
    public static ContractRegistered Build(string aggregateId)
    {
        var contractNumber = aggregateId;
        var customerNumber = "C72856";
        var productNumber = "FAC-00241";
        decimal amount = 100000;
        DateTime startDate = new DateTime(2022, 4, 24, 13, 47, 26);
        DateTime endDate = new DateTime(2027, 4, 24, 18, 33, 5);
 
        return new ContractRegistered(
            contractNumber,
            customerNumber,
            productNumber,
            amount,
            startDate,
            endDate);
    }
}