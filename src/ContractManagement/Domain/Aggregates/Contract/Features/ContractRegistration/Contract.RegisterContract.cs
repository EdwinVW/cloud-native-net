namespace ContractManagement.Domain.Aggregates.ContractAggregate
{
    public partial class Contract
    {
        public async ValueTask RegisterContractAsync(
            RegisterContractV2 command,
            ICustomerService customerService,
            IProductService productService)
        {
            await CheckBusinessRules(command, customerService, productService);
            if (!IsConsistent)
            {
                return;
            }

            // handle command
            var contractRegistered = ContractRegisteredV2.CreateFrom(command);
            ApplyDomainEvent(contractRegistered);
        }

        private void Handle(ContractRegisteredV2 domainEvent)
        {
            CustomerNumber = CustomerNumber.Parse(domainEvent.CustomerNumber);
            ProductNumber = ProductNumber.Parse(domainEvent.ProductNumber);
            Amount = MoneyAmount.Parse(domainEvent.Amount);
            ContractTerm = Duration.Parse(domainEvent.StartDate, domainEvent.EndDate);
            PaymentPeriod = domainEvent.PaymentPeriod;
        }

        private async ValueTask CheckBusinessRules(
            RegisterContractV2 command,
            ICustomerService customerService,
            IProductService productService)
        {
            if (!await customerService.IsExistingCustomerAsync(command.CustomerNumber))
            {
                AddBusinessRuleViolation($"Customer with customer-number {command.CustomerNumber} not found.");
            }
            if (!await productService.IsExistingProductAsync(command.ProductNumber))
            {
                AddBusinessRuleViolation($"Product with product-number {command.ProductNumber} not found.");
            }            
        }
    }
}