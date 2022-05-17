namespace ContractManagement.Infrastructure.Persistence.EFCore.Repositories.Aggregate;

public class EFAggregateRepository<TId, TAggregateRoot> : IAggregateRepository<TId, TAggregateRoot> 
    where TAggregateRoot : class, IAggregateRoot<TId>
{
    private readonly DbSet<TAggregateRoot> _aggregateSet;
    private readonly ILogger _logger;

    public EFAggregateRepository(ServiceDbContext context, ILogger<EFAggregateRepository<TId, TAggregateRoot>> logger)
    {
        _aggregateSet = context.Set<TAggregateRoot>();
        _logger = logger;
    }

    public async ValueTask<TAggregateRoot?> GetAggregateAsync(TId aggregateId)
    {
        return await _aggregateSet.FindAsync(aggregateId);
    }

    public ValueTask AddAggregateAsync(TAggregateRoot aggregate)
    {
        _logger.LogInformation("Adding {AggregateType} aggregate '{AggregateId}'...",
            typeof(TAggregateRoot).Name,
            aggregate.Id);

        _aggregateSet.Add(aggregate);

        return ValueTask.CompletedTask;
    }

    public ValueTask UpdateAggregateAsync(TAggregateRoot aggregate)
    {
        _logger.LogInformation("Updating {AggregateType} aggregate '{AggregateId}'...",
            typeof(TAggregateRoot).Name,
            aggregate.Id);

        // Update Aggregate
        // Because we've just queried the aggregate and it's tracked,
        // the original version number is in the context. We update it
        // to the new version number and get a optimistic concurrency check
        // because OriginalVersion is marked as a concurrency token.
        if (aggregate.Version is null)
        {
            aggregate.Version = 0;
        }
        else
        {
            aggregate.Version = aggregate.Version!.Value + 1;
        }

        _aggregateSet.Update(aggregate);

        return ValueTask.CompletedTask;
    }
}