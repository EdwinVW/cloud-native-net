namespace ContractManagement.Application.Interfaces;

public interface IProductReadModelRepository
{
    ValueTask AddProductAsync(ReadModels.Product product);

    ValueTask<ReadModels.Product?> GetProductByProductNumberAsync(string productNumber);
}