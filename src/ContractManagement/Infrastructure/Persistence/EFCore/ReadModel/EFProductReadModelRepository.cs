using ContractManagement.Application.ReadModels;

namespace ContractManagement.Infrastructure.Persistence.EFCore.Repositories;

public class EFProductReadModelRepository : IProductReadModelRepository
{
    private readonly ILogger<EFProductReadModelRepository> _logger;
    private readonly ServiceDbContext _dbContext;

    public EFProductReadModelRepository(ILogger<EFProductReadModelRepository> logger, ServiceDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async ValueTask<Product?> GetProductByProductNumberAsync(string productNumber)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(c => c.ProductNumber == productNumber);
    }

    public async ValueTask AddProductAsync(Product product)
    {
        // This readmodel contains data from another domain. Therefore we will not envorce unique constraints on the data.
        // The ProductRegistered event is handled as an upsert. So if a Product with the specified ProductNumber already 
        // exists in the database, its data is updated.

        var existingProduct = await _dbContext.Products.FirstOrDefaultAsync(c => c.ProductNumber == product.ProductNumber);

        if (existingProduct == null)
        {
            _dbContext.Products.Add(product);
        }
        else
        {
            existingProduct.Description = product.Description;
        }

        _logger.LogInformation("Added product {productNumber}.", product.ProductNumber);
    }

    public async ValueTask SaveChangeAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
