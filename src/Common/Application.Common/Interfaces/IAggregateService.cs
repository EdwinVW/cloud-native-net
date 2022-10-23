namespace Application.Common.Interfaces;

public interface IAggregateService<TAggregateRoot>
    where TAggregateRoot : IAggregateRoot
{
    ValueTask<TAggregateRoot> RehydrateAsync(string aggregateId, AggregateVersion? expectedVersion);

    ValueTask<TAggregateRoot?> TryRehydrateAsync(string aggregateId, AggregateVersion? expectedVersion);

    ValueTask ProcessChangesAsync(TAggregateRoot aggregate);
}
