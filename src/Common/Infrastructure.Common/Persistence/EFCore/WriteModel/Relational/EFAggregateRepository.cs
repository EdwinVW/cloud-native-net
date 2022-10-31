namespace Infrastructure.Common.Persistence.EFCore.Repositories.Aggregate;

public class EFAggregateRepository<TAggregateRoot> : IAggregateRepository<TAggregateRoot> 
    where TAggregateRoot : class, IAggregateRoot
{
    private readonly DbSet<TAggregateRoot> _aggregateSet;
    private readonly ILogger _logger;

    public EFAggregateRepository(DbContext context, ILogger<EFAggregateRepository<TAggregateRoot>> logger)
    {
        _aggregateSet = context.Set<TAggregateRoot>();
        _logger = logger;
    }

    public async ValueTask<TAggregateRoot?> GetAggregateAsync(string aggregateId)
    {
        return await _aggregateSet.FindAsync(aggregateId);
    }

    public ValueTask AddAggregateAsync(TAggregateRoot aggregate)
    {
        _logger.LogInformation("Adding {AggregateType} aggregate '{AggregateId}'...",
            typeof(TAggregateRoot).Name,
            aggregate.Id);

        aggregate.Version = 1;
        _aggregateSet.Add(aggregate);

        return ValueTask.CompletedTask;
    }

    public ValueTask UpdateAggregateAsync(TAggregateRoot aggregate)
    {
        _logger.LogInformation("Updating {AggregateType} aggregate '{AggregateId}'...",
            typeof(TAggregateRoot).Name,
            aggregate.Id);

        // Update Aggregate
        aggregate.Version += 1;

        _aggregateSet.Update(aggregate);

        return ValueTask.CompletedTask;
    }
}