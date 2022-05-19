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

    public Contract(EventSourcedEntityId id) : base(id)
    {
        ContractNumber = ContractNumber.Parse(id.Value);
    }

    public Contract(EventSourcedEntityId id, IList<Event> domainEvents) : base(id, domainEvents)
    {
        ContractNumber = ContractNumber.Parse(id.Value);
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
}