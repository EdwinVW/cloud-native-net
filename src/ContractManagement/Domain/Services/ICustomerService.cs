namespace ContractManagement.Domain.Services;

public interface ICustomerService
{
    ValueTask<bool> IsExistingCustomerAsync(string customerNumber);
}
