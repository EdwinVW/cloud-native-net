namespace ContractManagement.Application.Interfaces;

public interface ICustomerReadModelRepository
{
    Task AddCustomerAsync(ReadModels.Customer customer);

    ValueTask<ReadModels.Customer?> GetCustomerByCustomerNumberAsync(string customerNumber);
}