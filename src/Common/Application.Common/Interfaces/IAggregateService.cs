namespace Application.Common.Interfaces;

public interface IAggregateService<TAggregateRoot>
    where TAggregateRoot : IAggregateRoot
{
    ValueTask<TAggregateRoot> RehydrateAsync(string aggregateId, uint? expectedVersion);

    ValueTask<TAggregateRoot?> TryRehydrateAsync(string aggregateId, uint? expectedVersion);

    ValueTask ProcessChangesAsync(TAggregateRoot aggregate);
}
