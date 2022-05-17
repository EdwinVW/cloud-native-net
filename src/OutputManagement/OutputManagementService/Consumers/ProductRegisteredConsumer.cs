using ProductManagement.Application.IntegrationEvents;

namespace OutputManagementService.Consumers
{
    public class OutputManagement_ProductRegisteredConsumer : MassTransitConsumer<ProductRegistered>
    {
        private readonly IProductRepository _productRepository;

        public OutputManagement_ProductRegisteredConsumer(
            ILogger<OutputManagement_ProductRegisteredConsumer> logger,
            IProductRepository productRepository) : base(logger)
        {
            this._productRepository = productRepository;
        }

        protected override async Task ConsumeMessage(ProductRegistered message)
        {
            await _productRepository.AddProductAsync(
                new Product(message.ProductNumber, message.Description));
        }
    }
}