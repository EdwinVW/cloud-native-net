namespace ContractManagement.Infrastructure.Persistence.EFCore.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Account")
            .HasKey(e => e.AccountNumber);

        builder
            .Property(e => e.Balance)
            .HasPrecision(10, 2);

        builder
            .HasData(new Account[]
            {
                new Account
                {
                    AccountNumber = "CTR-20220502-9999",
                    Balance = 12500,
                    Version = 1
                }
            });
    }
}
