namespace ContractManagement.Application.EventHandlers.IntegrationEvents;

public class ProductRegisteredHandler : IEventHandler<ProductRegistered>
{
    private readonly IProductReadModelRepository _readModelRepository;

    public ProductRegisteredHandler(IProductReadModelRepository readModelRepository)
    {
        _readModelRepository = readModelRepository;
    }

    public async ValueTask HandleAsync(ProductRegistered integrationEvent)
    {
        await _readModelRepository.AddProductAsync(new ReadModels.Product
        {
            ProductNumber = integrationEvent.ProductNumber,
            Description = integrationEvent.Description
        });
    }
}