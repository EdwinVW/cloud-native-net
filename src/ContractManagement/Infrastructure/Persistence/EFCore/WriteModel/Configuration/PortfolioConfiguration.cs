namespace ContractManagement.Infrastructure.Persistence.EFCore.Configurations;

public class PortfolioConfiguration : IEntityTypeConfiguration<Portfolio>
{
    public void Configure(EntityTypeBuilder<Portfolio> builder)
    {
        builder.ToTable("Portfolio")
            .HasKey(e => e.PortfolioId);

        builder
            .HasMany<Document>(e => e.Documents)
            .WithOne()
            .HasForeignKey(e => e.PortfolioId);

        builder
            .HasData(new Portfolio[]
            {
                new Portfolio
                {
                    PortfolioId = "CTR-20220502-9999",
                    Version = 1
                }
            });
    }
}
