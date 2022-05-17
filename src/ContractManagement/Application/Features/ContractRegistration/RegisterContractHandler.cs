namespace Contractmanagement.Features.ContractRegistration;

public class RegisterContractHandler : ICommandHandler<RegisterContractV2>
{
    private readonly IAggregateService<EventSourcedEntityId, Contract> _aggregateService;
    private readonly ICustomerService _customerService;
    private readonly IProductService _productService;

    public RegisterContractHandler(
        IAggregateService<EventSourcedEntityId, Contract> aggregateService,
        ICustomerService customerService,
        IProductService productService)
    {
        _aggregateService = aggregateService;
        _customerService = customerService;
        _productService = productService;
    }

    public async ValueTask HandleAsync(RegisterContractV2 command)
    {
        // The Contract aggregate may or may not exist at this point.
        // First try to rehydrate it from the event store.
        var contract = await _aggregateService.TryRehydrateAsync(command.ContractNumber, command.ExpectedVersion);

        // If the aggregate wasn't found, create a new one
        if (contract is null)
        {
            contract = new Contract(command.ContractNumber);
        }

        await contract.RegisterContractAsync(command, _customerService, _productService);
        await _aggregateService.ProcessChangesAsync(contract);
    }
}