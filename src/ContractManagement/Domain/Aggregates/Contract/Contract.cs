namespace ContractManagement.Domain.Aggregates.ContractAggregate;

public partial class Contract : EventSourcedAggregateRoot
{
    public ContractNumber ContractNumber { get; init; }

    public CustomerNumber? CustomerNumber { get; private set; }

    public ProductNumber? ProductNumber { get; private set; }

    public MoneyAmount? Amount { get; private set; }

    public Duration? ContractTerm { get; private set; }

    public PaymentPeriod PaymentPeriod { get; private set; }

    public bool Cancelled { get; private set; } = false;

    #region Constructors

    public Contract(EventSourcedEntityId id) : base(id)
    {
        ContractNumber = ContractNumber.Parse(id.Value);
    }

    public Contract(EventSourcedEntityId id, IList<Event> domainEvents) : base(id, domainEvents)
    {
        ContractNumber = ContractNumber.Parse(id.Value);
    }

    #endregion

    #region Commandhandling

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

    public ValueTask ChangeContractAmount(ChangeContractAmount command)
    {
        EnsureNotCancelled();
        EnsureValidAmount(command.NewAmount);

        if (IsValid)
        {
            var contractAmountChanged = ContractAmountChanged.CreateFrom(command);
            ApplyDomainEvent(contractAmountChanged);
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask ChangeContractTerm(ChangeContractTerm command)
    {
        EnsureNotCancelled();
        EnsureValidTerm(command.StartDate, command.EndDate);

        if (IsValid)
        {
            var contractTermChanged = ContractTermChanged.CreateFrom(command);
            ApplyDomainEvent(contractTermChanged);
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask CancelContract(CancelContract command)
    {
        EnsureNotCancelled();
        EnsureValidTermForCancellation();
        if (IsValid)
        {
            var contractCancelled = ContractCancelled.CreateFrom(command);
            ApplyDomainEvent(contractCancelled);
        }
        return ValueTask.CompletedTask;
    }

    #endregion

    #region Eventhandling

    private void Handle(ContractRegisteredV2 domainEvent)
    {
        CustomerNumber = CustomerNumber.Parse(domainEvent.CustomerNumber);
        ProductNumber = ProductNumber.Parse(domainEvent.ProductNumber);
        Amount = MoneyAmount.Parse(domainEvent.Amount);
        ContractTerm = Duration.Parse(domainEvent.StartDate, domainEvent.EndDate);
        PaymentPeriod = domainEvent.PaymentPeriod;
    }

    protected override bool TryHandleDomainEvent(Event domainEvent)
    {
        // Upgrade events to latest version
        if (domainEvent is ContractRegistered)
        {
            domainEvent = ContractRegisteredV2.CreateFrom((ContractRegistered)domainEvent);
        }

        // Handle event
        switch (domainEvent)
        {
            case ContractRegisteredV2 contractRegisteredV2:
                Handle(contractRegisteredV2);
                return true;

            case ContractAmountChanged contractAmountChanged:
                Handle(contractAmountChanged);
                return true;

            case ContractTermChanged contractTermChanged:
                Handle(contractTermChanged);
                return true;

            case ContractCancelled contractCancelled:
                Handle(contractCancelled);
                return true;

            default:
                return false;
        }
    }

    private void Handle(ContractAmountChanged domainEvent)
    {
        Amount = MoneyAmount.Parse(domainEvent.NewAmount);
    }

    private void Handle(ContractTermChanged domainEvent)
    {
        ContractTerm = Duration.Parse(domainEvent.StartDate, domainEvent.EndDate);
    }

    private void Handle(ContractCancelled domainEvent)
    {
        Cancelled = true;
    }

    #endregion

    #region Business Rules

    private async Task EnsureExistingProduct(string productNumber, IProductService productService)
    {
        if (!await productService.IsExistingProductAsync(productNumber))
        {
            AddBusinessRuleViolation($"Product with product-number {productNumber} not found.");
        }
    }

    private async Task EnsureExistingCustomer(string customerNumber, ICustomerService customerService)
    {
        if (!await customerService.IsExistingCustomerAsync(customerNumber))
        {
            AddBusinessRuleViolation($"Customer with customer-number {customerNumber} not found.");
        }
    }

    private void EnsureNotCancelled()
    {
        if (Cancelled)
        {
            AddBusinessRuleViolation("The contract was cancelled.");
        }
    }

    private void EnsureValidAmount(decimal amount)
    {
        // Contract amount must be between 1000 and 10000000
        if (amount < 1000 || amount > 10000000)
        {
            AddBusinessRuleViolation(
                "Invalid amount. The amount on a contract must be between 1.000 and 10.000.000 Euros.");
        }
    }

    private void EnsureValidTerm(DateTime startDate, DateTime endDate)
    {
        // Contract term must be at least 5 years
        if (endDate < startDate.Date.AddYears(5))
        {
            AddBusinessRuleViolation(
                "Invalid contract term. The term should be at least 5 years.");
        }

        // Contract term must be no longer than 50 years
        if (endDate > startDate.Date.AddYears(50))
        {
            AddBusinessRuleViolation(
                "Invalid contract term. The term should be no longer than 50 years.");
        }
    }

    private void EnsureValidTermForCancellation()
    {
        if (DateTime.Now.Date >= ContractTerm?.EndDate.Date.AddYears(-3))
        {
            AddBusinessRuleViolation("Contract can not be cancelled if it is within 3 years from the end of its term.");
        }
    }

    /// <summary>
    /// Check aggregate consistency after the changes have been made (post-validation).
    /// </summary>
    public override void EnsureConsistency()
    {
        // A yearly PaymentPeriod is only allowed for contracts below 5.000.000 euros
        if (PaymentPeriod == PaymentPeriod.Yearly && Amount?.Value >= 5000000)
        {
            AddBusinessRuleViolation(
                "Invalid PaymentPeriod. PaymentPeriod 'Yearly' is only allowed for contracts below 5.000.000 euros.");
        }
    }

    #endregion
}