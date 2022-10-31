namespace Domain.UnitTests.TestDataBuilders.Events;

public class ContractCancelledBuilder
{
    public static ContractCancelled Build(string aggregateId)
    {
        var contractNumber = aggregateId;
        var reason = "Divorce";
 
        return new ContractCancelled(
            contractNumber,
            reason);
    }
}