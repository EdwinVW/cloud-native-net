namespace ContractManagement.Application.EventHandlers.IntegrationEvents;

public class CustomerRegisteredHandler : IEventHandler<CustomerRegistered>
{
    private readonly ICustomerReadModelRepository _readModelRepository;

    public CustomerRegisteredHandler(ICustomerReadModelRepository readModelRepository)
    {
        _readModelRepository = readModelRepository;
    }

    public async ValueTask HandleAsync(CustomerRegistered integrationEvent)
    {
        await _readModelRepository.AddCustomerAsync(new ReadModels.Customer
        {
            CustomerNumber = integrationEvent.CustomerNumber,
            Name = $"{integrationEvent.FirstName.Trim()} {integrationEvent.LastName.Trim()}",
            Address = integrationEvent.Address,
            EmailAddress = integrationEvent.Email
        });
    }
}


