namespace Domain.UnitTests.TestDataBuilders.Commands;

public class ChangeContractAmountBuilder
{
    public static ChangeContractAmount Build()
    {
        var _contractNumber = "CTR-20220424-0001";
        decimal _newAmount = 200000;
 
        return new ChangeContractAmount(
            _contractNumber,
            _newAmount);
    }
}