namespace OutputManagementService.Repositories
{
    public class InMemoryProductRepository : IProductRepository
    {
        private readonly ILogger<InMemoryProductRepository> _logger;

        public InMemoryProductRepository(ILogger<InMemoryProductRepository> logger)
        {
            this._logger = logger;

        }
        private IList<Product> _Products = new List<Product>
        {
            new Product("FAC-00011", "Standard long term loan")
        };

        public Task AddProductAsync(Product Product)
        {
            var existingProduct = _Products.FirstOrDefault(c => c.ProductNumber == Product.ProductNumber);
            if (existingProduct != null)
            {
                _Products.Remove(existingProduct);
            }
            _Products.Add(Product);

            _logger.LogInformation("Added Product {productNumber}.", Product.ProductNumber);
            
            return Task.CompletedTask;
        }

        public Task<Product?> GetProductByProductNumberAsync(string ProductNumber)
        {
            return Task.FromResult(_Products.FirstOrDefault(c => c.ProductNumber == ProductNumber));
        }
    }
}