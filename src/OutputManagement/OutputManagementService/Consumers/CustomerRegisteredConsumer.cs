using CustomerManagement.Application.IntegrationEvents;

namespace OutputManagementService.Consumers;

public class OutputManagement_CustomerRegisteredConsumer : MassTransitConsumer<CustomerRegistered>
{
    private readonly ICustomerRepository _customerRepository;

    public OutputManagement_CustomerRegisteredConsumer(
        ILogger<OutputManagement_CustomerRegisteredConsumer> logger,
        ICustomerRepository customerRepository) : base(logger)
    {
        this._customerRepository = customerRepository;
    }

    protected override async Task ConsumeMessage(CustomerRegistered message)
    {
        await _customerRepository.AddCustomerAsync(
            new Customer(message.CustomerNumber, message.Email));
    }
}
