namespace ContractManagement.Application.Services;

public class CustomerService : IDomainService, ICustomerService
{
    private readonly ICustomerReadModelRepository _repository;

    public CustomerService(ICustomerReadModelRepository repository)
    {
        _repository = repository;
    }
    
    public async ValueTask<bool> IsExistingCustomerAsync(string customerNumber)
    {
        return await _repository.GetCustomerByCustomerNumberAsync(customerNumber) != null;
    }
}
