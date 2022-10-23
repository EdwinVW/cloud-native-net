using System.Text;

namespace Application.Common.Services;

public class AggregateService<TAggregateRoot> : IAggregateService<TAggregateRoot>
    where TAggregateRoot : IAggregateRoot
{
    private readonly IAggregateRepository<TAggregateRoot> _repository;
    private readonly IProjectionEngine _projectionEngine;
    private readonly IEventPublisher _eventPublisher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public AggregateService(
        IAggregateRepository<TAggregateRoot> repository,
        IProjectionEngine projectionEngine,
        IEventPublisher eventPublisher,
        IUnitOfWork unitOfWork,
        ILoggerFactory loggerFactory)
    {
        _repository = repository;
        _projectionEngine = projectionEngine;
        _eventPublisher = eventPublisher;
        _unitOfWork = unitOfWork;
        _logger = loggerFactory.CreateLogger<AggregateService<TAggregateRoot>>();
    }

    public async ValueTask<TAggregateRoot> RehydrateAsync(
        string aggregateId,
        uint? expectedVersion)
    {
        var aggregate = await TryRehydrateAsync(aggregateId, expectedVersion);
        if (aggregate is null)
        {
            throw new ArgumentException($"Cannot find aggregate with Id '{aggregateId}'.", nameof(aggregateId));
        }

        return aggregate;
    }

    public async ValueTask<TAggregateRoot?> TryRehydrateAsync(
        string aggregateId,
        uint? expectedVersion)
    {
        var aggregate = await _repository.GetAggregateAsync(aggregateId);
        if (aggregate is not null)
        {
            if (expectedVersion is not null && expectedVersion != aggregate.Version)
            {
                throw new ConcurrencyException($"Expected version '{expectedVersion}', but found version '{aggregate.Version}'.");
            }

            return aggregate;
        }

        return default(TAggregateRoot);
    }

    public async ValueTask ProcessChangesAsync(TAggregateRoot aggregate)
    {
        // Before saving the aggregate, make sure it it is consistent
        if (!aggregate.IsValid)
        {
            var exception = new BusinessRuleViolationException(
                "The handling of the command left the aggregate in an inconsistent state.");
            foreach (var violation in aggregate.GetBusinessRuleViolations())
            {
                exception.AddViolation(violation);
            }
            throw exception;
        }

        // Update write model
        if (aggregate.IsNew)
        {
            await _repository.AddAggregateAsync(aggregate);
        }
        else
        {
            await _repository.UpdateAggregateAsync(aggregate);
        }
    
        // Process domain event(s)
        var domainEvents = aggregate.GetDomainEvents();
        if (domainEvents.Any())
        {
            // Update read model(s)
            await _projectionEngine.RunProjectionsAsync(domainEvents);

            // Register domain event(s) to publish
            _unitOfWork.AddDomainEventsToPublish(domainEvents);
        }
    }
}
