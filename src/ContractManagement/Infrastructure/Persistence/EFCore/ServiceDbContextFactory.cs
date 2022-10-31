namespace ContractManagement.Infrastructure.Persistence.EFCore;

public class ServiceDbContextFactory : IDesignTimeDbContextFactory<ServiceDbContext>
{
    public ServiceDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ServiceDbContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CloudNativeNet;Trusted_Connection=True;");

        return new ServiceDbContext(optionsBuilder.Options);
    }
}
