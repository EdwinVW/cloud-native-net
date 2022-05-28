namespace Domain.UnitTests.TestDataBuilders.Commands;

public class RegisterContractV2Builder
{
    public static RegisterContractV2 Build(string aggregateId)
    {
        var _contractNumber = aggregateId;
        var _customerNumber = "C72856";
        var _productNumber = "FAC-00241";
        decimal _amount = 100000;
        DateTime _startDate = new DateTime(2022, 4, 24, 13, 47, 26);
        DateTime _endDate = new DateTime(2032, 4, 24, 18, 33, 5);
        PaymentPeriod _paymentPeriod = PaymentPeriod.Monthly;
 
        return new RegisterContractV2(
            _contractNumber,
            _customerNumber,
            _productNumber,
            _amount,
            _startDate,
            _endDate,
            _paymentPeriod);
    }
}