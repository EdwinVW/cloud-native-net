using ContractManagement.Application.ReadModels;

namespace ContractManagement.Infrastructure.Persistence.EFCore.Repositories;

public class EFCustomerReadModelRepository : ICustomerReadModelRepository
{
    private readonly ILogger<EFCustomerReadModelRepository> _logger;
    private readonly ServiceDbContext _dbContext;

    public EFCustomerReadModelRepository(ILogger<EFCustomerReadModelRepository> logger, ServiceDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async ValueTask<Customer?> GetCustomerByCustomerNumberAsync(string customerNumber)
    {
        return await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerNumber == customerNumber);
    }

    public async Task AddCustomerAsync(Customer customer)
    {
        // This readmodel contains data from another domain. Therefore we will not envorce unique constraints on the data.
        // The CustomerRegistered event is handled as an upsert. So if a customer with the specified CustomerNumber already 
        // exists in the database, its data is updated.

        var existingCustomer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.CustomerNumber == customer.CustomerNumber);

        if (existingCustomer == null)
        {
            _dbContext.Customers.Add(customer);
        }
        else
        {
            existingCustomer.Name = customer.Name;
            existingCustomer.Address = customer.Address;
            existingCustomer.EmailAddress = customer.EmailAddress;
        }

        _logger.LogInformation("Added customer {customerNumber}.", customer.CustomerNumber);
    }
}
