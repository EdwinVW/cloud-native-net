using Generic.IntegrationEvents;

namespace OutputManagementService.Consumers;

public class OutputManagement_DayHasPassedConsumer : MassTransitConsumer<DayHasPassed>
{
    private readonly IEmailService _emailService;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;
    private readonly IContractRepository _contractRepository;

    public OutputManagement_DayHasPassedConsumer(
        ILogger<OutputManagement_DayHasPassedConsumer> logger,
        IEmailService emailService,
        ICustomerRepository customerRepository,
        IProductRepository productRepository,
        IContractRepository contractRepository) : base(logger)
    {
        _emailService = emailService;
        _customerRepository = customerRepository;
        _productRepository = productRepository;
        _contractRepository = contractRepository;
    }

    protected override async Task ConsumeMessage(DayHasPassed message)
    {
        var today = DateOnly.FromDateTime(DateTime.Now.Date);
        var contracts = await _contractRepository.GetContractsToSendAsync(today);
        foreach (var contract in contracts.ToList())
        {
            // get product
            var product = await _productRepository.GetProductByProductNumberAsync(contract.ProductNumber);
            if (product == null)
            {
                // we could expand this with a call to the Product Management service
                _logger.LogError($"No Product found with Product number '{contract.ProductNumber}'. Unable to send contract.");
                return;
            }

            // get customer
            var customer = await _customerRepository.GetCustomerByCustomerNumberAsync(contract.CustomerNumber);
            if (customer == null)
            {
                // we could expand this with a call to the Customer Management service
                _logger.LogError($"No customer found with customer number '{contract.CustomerNumber}'. Unable to send contract.");
                return;
            }

            // send contract
            await _emailService.SendContract(new ContractInfo(
                contract.ContractNumber,
                product!.ProductNumber,
                product!.ProductDescription,
                customer!.EmailAddress));

            // mark contract as sent
            await _contractRepository.MarkContractAsSentAsync(contract.ContractNumber);
        }
    }
}
