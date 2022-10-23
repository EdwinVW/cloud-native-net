namespace Application.Common.Interfaces;

public interface IAggregateRepository<TAggregateRoot>
    where TAggregateRoot : IAggregateRoot
{
    ValueTask<TAggregateRoot?> GetAggregateAsync(string aggregateId);

    ValueTask AddAggregateAsync(TAggregateRoot aggregate);

    ValueTask UpdateAggregateAsync(TAggregateRoot aggregate);
}
