namespace ContractManagement.Domain.Aggregates.ContractAggregate
{
    public partial class Contract
    {
        public async ValueTask RegisterContractAsync(
            RegisterContractV2 command,
            ICustomerService customerService,
            IProductService productService)
        {
            EnsureValidAmount(command.Amount);
            EnsureValidTerm(command.StartDate, command.EndDate);
            await EnsureExistingCustomer(command.CustomerNumber, customerService);
            await EnsureExistingProduct(command.ProductNumber, productService);

            if (IsValid)
            {
                // handle command
                var contractRegistered = ContractRegisteredV2.CreateFrom(command);
                ApplyDomainEvent(contractRegistered);
            }
        }

        private void Handle(ContractRegisteredV2 domainEvent)
        {
            CustomerNumber = CustomerNumber.Parse(domainEvent.CustomerNumber);
            ProductNumber = ProductNumber.Parse(domainEvent.ProductNumber);
            Amount = MoneyAmount.Parse(domainEvent.Amount);
            ContractTerm = Duration.Parse(domainEvent.StartDate, domainEvent.EndDate);
            PaymentPeriod = domainEvent.PaymentPeriod;
        }
    }
}