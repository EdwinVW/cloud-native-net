namespace Infrastructure.Common.Persistence.EFCore.Repositories.Aggregate;

public class AggregateEntity
{
    public string AggregateId { get; set; } = default!;

    public uint Version { get; set; } = default!;
}