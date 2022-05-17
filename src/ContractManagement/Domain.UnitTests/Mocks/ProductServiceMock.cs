namespace Domain.UnitTests.Mocks
{
    public class ProductServiceMock
    {
        public static Mock<IProductService> ForExistingProduct(string productNumber)
        {
            return CreateMock(productNumber, true);
        }

        public static Mock<IProductService> ForNonExistingProduct(string productNumber)
        {
            return CreateMock(productNumber, false);
        }            

        private static Mock<IProductService> CreateMock(string productNumber, bool exists)
        {
            var mock = new Mock<IProductService>();
            mock
                .Setup(m => m.IsExistingProductAsync(productNumber))
                .Returns(ValueTask.FromResult(exists));
            return mock;
        }      
    }
}