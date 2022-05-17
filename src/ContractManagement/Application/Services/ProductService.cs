namespace ContractManagement.Application.Services;

public class ProductService : IDomainService, IProductService
{
    private readonly IProductReadModelRepository _repository;

    public ProductService(IProductReadModelRepository repository)
    {
        _repository = repository;

    }
    public async ValueTask<bool> IsExistingProductAsync(string productNumber)
    {
        return await _repository.GetProductByProductNumberAsync(productNumber) != null;
    }
}
