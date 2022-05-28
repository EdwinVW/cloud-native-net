namespace Domain.UnitTests.TestDataBuilders.Commands;

public class CancelContractBuilder
{
    public static CancelContract Build(string aggregateId)
    {
        string _contractNumber = aggregateId;
        string _reason = "Contract replaced with another contract.";
 
        return new CancelContract(
            _contractNumber,
            _reason);
    }
}