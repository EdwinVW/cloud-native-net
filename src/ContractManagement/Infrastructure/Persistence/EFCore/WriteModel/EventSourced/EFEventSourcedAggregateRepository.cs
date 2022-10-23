namespace ContractManagement.Infrastructure.Persistence.EFCore.Repositories.Aggregate;

public class EFEventSourcedAggregateRepository<TAggregateRoot> : 
    IAggregateRepository<TAggregateRoot>
        where TAggregateRoot : AggregateRoot
{
    private readonly string _eventTypeFormatString;

    private readonly DbSet<AggregateEntity> _aggregateSet;
    private readonly DbSet<EventEntity> _eventSet;
    private readonly ILogger _logger;

    public EFEventSourcedAggregateRepository(
        ServiceDbContext context,
        ILogger<EFEventSourcedAggregateRepository<TAggregateRoot>> logger)
    {
        var aggregateType = typeof(TAggregateRoot);

        _aggregateSet = context.Set<AggregateEntity>($"{aggregateType.Name}Aggregate");
        _eventSet = context.Set<EventEntity>($"{aggregateType.Name}Event");
        _eventTypeFormatString = $"{aggregateType.Namespace}.DomainEvents.{{0}}, {aggregateType.Assembly.GetName()}";
        _logger = logger;
    }

    public async ValueTask<TAggregateRoot?> GetAggregateAsync(string aggregateId)
    {
        var domainEvents = await _eventSet
            .Where(a => a.AggregateId == aggregateId)
            .Select(e => DeserializeEvent(e, _eventTypeFormatString))
            .ToListAsync();

        _logger.LogDebug("Retrieved events {@events}", domainEvents);

        return domainEvents.Any() ? 
            RehydrateAggregate(aggregateId, domainEvents) : 
            default(TAggregateRoot);
    }

    public ValueTask AddAggregateAsync(TAggregateRoot aggregate)
    {
        _logger.LogInformation("Adding {AggregateType} aggregate '{AggregateId}'...",
            typeof(TAggregateRoot).Name,
            aggregate.Id);

        if (aggregate.GetDomainEvents().Any())
        {
            AddAggregateEntity(aggregate);
            AddEventEntities(aggregate);
        }

        return ValueTask.CompletedTask;
    }

    public async ValueTask UpdateAggregateAsync(TAggregateRoot aggregate)
    {
        _logger.LogInformation("Updating {AggregateType} aggregate '{AggregateId}'...",
            typeof(TAggregateRoot).Name,
            aggregate.Id);

        if (aggregate.GetDomainEvents().Any())
        {
            await UpdateAggregateEntityAsync(aggregate);

            AddEventEntities(aggregate);
        }
    }

    private void AddAggregateEntity(TAggregateRoot aggregate)
    {
        AggregateEntity newAggregate = new()
        {
            AggregateId = aggregate.Id,
            Version = (uint)aggregate.GetDomainEvents().Count()
        };

        _aggregateSet.Add(newAggregate);
    }

    private async ValueTask UpdateAggregateEntityAsync(TAggregateRoot aggregate)
    {
        var aggregateEntity = await _aggregateSet
            .FirstOrDefaultAsync(a => a.AggregateId == aggregate.Id);

        if (aggregateEntity is not null)
        {
            // Make sure that the aggregate entity in the database is still
            // the version that we're trying to update.
            if (aggregateEntity.Version != aggregate.Version)
            {
                throw new ConcurrencyException(
                    $"Expected version '{aggregate.Version}', but found version '{aggregateEntity.Version}'.");
            }

            // Update Aggregate
            // Because we've just queried the aggregate and it's tracked,
            // the original version number is in the context. We update it
            // to the new version number and get a optimistic concurrency check
            // because Version is marked as a concurrency token.
            aggregateEntity.Version += (uint)aggregate.GetDomainEvents().Count();
        }
        else
        {
            // Concurrency exception because somebody deleted it?
            throw new Exception("BOOM! Cannot update non-existing aggregate");
        }
    }

    private void AddEventEntities(TAggregateRoot aggregate)
    {
        var nextEventVersion = aggregate.IsNew ? 1 : aggregate.Version! + 1;

        foreach (var domainEvent in aggregate.GetDomainEvents())
        {
            EventEntity newEvent = new()
            {
                AggregateId = aggregate.Id,
                Version = nextEventVersion++,
                Timestamp = DateTime.UtcNow,
                EventType = domainEvent.Type,
                EventData = JsonSerializer.Serialize(domainEvent, domainEvent.GetType())
            };

            _eventSet.Add(newEvent);
        }
    }

    private static TAggregateRoot RehydrateAggregate(
        string aggregateId, 
        IList<Event> domainEvents) =>
            (TAggregateRoot)Activator.CreateInstance(
                typeof(TAggregateRoot),
                aggregateId,
                domainEvents)!;

    /// <remarks>
    /// Method must be static because it's used in an EF Core Linq expression.
    /// </remarks>
    private static Event DeserializeEvent(EventEntity eventEntity, string eventTypeFormatString)
    {
        var eventTypeName = string.Format(eventTypeFormatString, eventEntity.EventType);
        var eventType = Type.GetType(eventTypeName);
        if (eventType is null)
        {
            throw new TypeLoadException(
                $"Failed to deserialize '{eventTypeName}' event." +
                $" Expected to find type '{eventTypeName}' based on aggregate type '{typeof(TAggregateRoot).FullName}'");
        }

        var domainEventObject = JsonSerializer.Deserialize(eventEntity.EventData, eventType);
        return (Event)domainEventObject!;
    }
}