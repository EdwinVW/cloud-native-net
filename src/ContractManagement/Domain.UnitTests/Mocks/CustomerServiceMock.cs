namespace Domain.UnitTests.Mocks
{
    public class CustomerServiceMock
    {
        public static Mock<ICustomerService> ForExistingCustomer(string customerNumber)
        {
            return CreateMock(customerNumber, true);
        }

        public static Mock<ICustomerService> ForNonExistingCustomer(string customerNumber)
        {
            return CreateMock(customerNumber, false);
        }            

        private static Mock<ICustomerService> CreateMock(string customerNumber, bool exists)
        {
            var mock = new Mock<ICustomerService>();
            mock
                .Setup(m => m.IsExistingCustomerAsync(customerNumber))
                .Returns(ValueTask.FromResult(exists));
            return mock;
        }    
    }
}