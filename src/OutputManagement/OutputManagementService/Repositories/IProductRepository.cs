namespace OutputManagementService.Repositories
{
    public interface IProductRepository
    {
        Task AddProductAsync(Product customer);
        Task<Product?> GetProductByProductNumberAsync(string productNumber);
    }
}