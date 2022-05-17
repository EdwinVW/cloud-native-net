namespace ContractManagement.Domain.Services;

public interface IProductService
{
    ValueTask<bool> IsExistingProductAsync(string productNumber);
}
