namespace Domain.UnitTests.TestDataBuilders.Commands;

public class ChangeContractTermBuilder
{
    public static ChangeContractTerm Build()
    {
        var _contractNumber = "CTR-20220424-0001";
        DateTime _startDate = new DateTime(2025, 1, 1, 13, 47, 26);
        DateTime _endDate = new DateTime(2035, 1, 1, 18, 33, 5);
 
        return new ChangeContractTerm(
            _contractNumber,
            _startDate,
            _endDate);
    }
}