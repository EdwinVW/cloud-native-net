namespace Domain.UnitTests.TestDataBuilders.Commands;

public class CancelContractBuilder
{
    public static CancelContract Build()
    {
        string _contractNumber = "CTR-20220424-0001";
        string _reason = "Contract replaced with another contract.";
 
        return new CancelContract(
            _contractNumber,
            _reason);
    }
}