namespace Application.Common.Interfaces;

public interface IAggregateRepository<TId, TAggregateRoot>
    where TAggregateRoot : IAggregateRoot<TId>
{
    ValueTask<TAggregateRoot?> GetAggregateAsync(TId aggregateId);

    ValueTask AddAggregateAsync(TAggregateRoot aggregate);

    ValueTask UpdateAggregateAsync(TAggregateRoot aggregate);
}
