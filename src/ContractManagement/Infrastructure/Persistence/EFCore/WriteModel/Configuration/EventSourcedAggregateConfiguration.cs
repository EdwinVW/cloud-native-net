namespace ContractManagement.Infrastructure.Persistence.EFCore.Configurations;

public static class EventSourcedAggregateConfiguration
{
    /// <summary>
    /// Configures an AggregateEntity for an event sourced aggregate.
    /// </summary>
    public static void ConfigureAggregateEntity(EntityTypeBuilder<AggregateEntity> builder)
    {
        builder.HasKey(e => e.AggregateId);

        builder
            .Property(e => e.AggregateId)
            .HasMaxLength(64);

        builder
            .Property(e => e.Version)
            .IsConcurrencyToken();

        if (builder.Metadata.GetTableName() == "ContractAggregate")
        {
            builder.HasData(new AggregateEntity[]
            {
                new AggregateEntity
                {
                    AggregateId = "CTR-20220502-9999",
                    Version = 1
                }
            });
        }
    }

    /// <summary>
    /// Configures an EventEntity for an event sourced aggregate.
    /// </summary>
    public static void ConfigureEventEntity(EntityTypeBuilder<EventEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(e => e.AggregateId)
            .HasMaxLength(64);

        builder
            .Property(e => e.Version);

        if (builder.Metadata.GetTableName() == "ContractEvent")
        {
            builder.HasData(new EventEntity[]
            {
                new EventEntity
                {
                    Id = Guid.NewGuid(),
                    AggregateId = "CTR-20220502-9999",
                    Version = 1,
                    Timestamp = DateTime.Now,
                    EventType = "ContractRegistered",
                    EventData = @"{""ContractNumber"": ""CTR-20220502-9999"",""CustomerNumber"": ""C13976"",""ProductNumber"": ""FAC-00011""," +
                                @"""Amount"": 20000,""StartDate"": ""2022-05-02T12:40:35.876Z"",""EndDate"": ""2034-05-02T12:40:35.877Z""," +
                                @"""EventId"": ""f0074479-4cea-41ff-a669-bdb3649f6e7b""}"
                }
            });
        }
    }
}