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

    protected override bool TryHandleDomainEvent(Event domainEvent, bool rehydrating)
    {
        // check overall pre-validation business rules
        // (only when not rehydrating state)
        if (!rehydrating)
        {
            if (Cancelled)
            {
                AddBusinessRuleViolation("The contract was cancelled.");
                return true;
            }
        }

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
    /// Check post-validation business rules.
    /// </summary>
    public override void EnsureConsistency()
    {
        // Contract amount must be between 1000 and 10000000
        if (Amount?.Value < 1000 || Amount?.Value > 10000000)
        {
            AddBusinessRuleViolation(
                "Invalid amount. The amount on a contract must be between 1.000 and 10.000.000 Euros.");
        }

        // Contract term must be at least 5 years
        if (ContractTerm?.EndDate < ContractTerm?.StartDate.Date.AddYears(5))
        {
            AddBusinessRuleViolation(
                "Invalid contract term. The term should be at least 5 years.");
        }

        // Contract term must be no longer than 50 years
        if (ContractTerm?.EndDate > ContractTerm?.StartDate.Date.AddYears(50))
        {
            AddBusinessRuleViolation(
                "Invalid contract term. The term should be no longer than 50 years.");
        }        

        // A yearly PaymentPeriod is only allowed for contracts below 5.000.000 euros
        if (PaymentPeriod == PaymentPeriod.Yearly && Amount?.Value >= 5000000)
        {
            AddBusinessRuleViolation(
                "Invalid PaymentPeriod. PaymentPeriod 'Yearly' is only allowed for contracts below 5.000.000 euros.");
        }        
    }
}