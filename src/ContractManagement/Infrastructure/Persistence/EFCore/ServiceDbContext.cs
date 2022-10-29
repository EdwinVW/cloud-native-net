using ReadModels = ContractManagement.Application.ReadModels;

namespace ContractManagement.Infrastructure.Persistence.EFCore;

public class ServiceDbContext : DbContext
{
    private const int SQL_ERROR_KEY_CONSTRAINT_VIOLATION = 2627;

    // Write models
    public DbSet<ContractManagement.Domain.Aggregates.Account.Account> Accounts => 
        Set<ContractManagement.Domain.Aggregates.Account.Account>();

    // Event Sourced write models 
    private static readonly string[] EventSourcedAggregates = new[]
    {
        nameof(ContractManagement.Domain.Aggregates.Contract.Contract)
    };

    // Read models
    public DbSet<ReadModels.Contract> Contracts => Set<ReadModels.Contract>();
    
    public DbSet<ReadModels.Customer> Customers => Set <ReadModels.Customer>();

    public DbSet<ReadModels.Product> Products => Set <ReadModels.Product>();

    public ServiceDbContext(DbContextOptions<ServiceDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure DbContext for event sourced aggregates.
        // You can remove this if the service does not use event sourcing.
        foreach (var aggregateName in EventSourcedAggregates)
        {
            modelBuilder.SharedTypeEntity<AggregateEntity>(
                $"{aggregateName}Aggregate",
                EventSourcedAggregateConfiguration.ConfigureAggregateEntity);

            modelBuilder.SharedTypeEntity<EventEntity>(
                $"{aggregateName}Event",
                EventSourcedAggregateConfiguration.ConfigureEventEntity);
        }

        // Configure DbContext for regular (non event sourced) aggregates and read models.
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
