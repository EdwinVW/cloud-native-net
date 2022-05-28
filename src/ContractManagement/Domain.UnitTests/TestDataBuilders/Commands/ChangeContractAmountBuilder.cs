namespace Domain.UnitTests.TestDataBuilders.Commands;

public class ChangeContractAmountBuilder
{
    public static ChangeContractAmount Build(string aggregateId)
    {
        var _contractNumber = aggregateId;
        decimal _newAmount = 200000;
 
        return new ChangeContractAmount(
            _contractNumber,
            _newAmount);
    }
}