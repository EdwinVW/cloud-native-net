namespace Application.Common.Interfaces;

public interface IAggregateService<TAggregateId, TAggregateRoot>
    where TAggregateRoot : IAggregateRoot<TAggregateId>
{
    ValueTask<TAggregateRoot> RehydrateAsync(TAggregateId aggregateId, AggregateVersion? expectedVersion);

    ValueTask<TAggregateRoot?> TryRehydrateAsync(TAggregateId aggregateId, AggregateVersion? expectedVersion);

    ValueTask ProcessChangesAsync(TAggregateRoot aggregate);
}
