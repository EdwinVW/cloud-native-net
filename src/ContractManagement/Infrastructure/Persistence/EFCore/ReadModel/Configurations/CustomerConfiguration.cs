using ReadModels = ContractManagement.Application.ReadModels;

namespace ContractManagement.Infrastructure.Persistence.EFCore.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<ReadModels.Customer>
{
    public void Configure(EntityTypeBuilder<ReadModels.Customer> builder)
    {
        builder.ToTable("Customer")
            .HasKey(e => e.CustomerNumber);

        builder
            .Property(e => e.CustomerNumber)
            .IsFixedLength()
            .HasMaxLength(6);

        builder
            .Property(e => e.Name)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder
            .Property(e => e.Address)
            .HasConversion<string>()
            .HasMaxLength(100);

        builder
            .Property(e => e.EmailAddress)
            .HasConversion<string>()
            .HasMaxLength(75);

        builder.HasData(new ReadModels.Customer[]
        {
            new ReadModels.Customer 
            {
                 CustomerNumber = "C13976",
                 Name = "John Doe",
                 Address = "Mainstreet 5463",
                 EmailAddress = "jd@example.com"
            },
            new ReadModels.Customer 
            {
                 CustomerNumber = "C13977",
                 Name = "Eric Dewitt",
                 Address = "First Av. 16743",
                 EmailAddress = "eric.dewitt@example.com"
            },            
        });
    }
}
