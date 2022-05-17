using Model = ContractManagement.Application.ReadModels;

namespace ContractManagement.Infrastructure.Persistence.EFCore.ReadModel.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Model.Product>
{
    public void Configure(EntityTypeBuilder<Model.Product> builder)
    {
        builder.ToTable("Product")
            .HasKey(e => e.ProductNumber);

        builder
            .Property(e => e.ProductNumber)
            .IsFixedLength()
            .HasMaxLength(18);

        builder
            .HasData(new Model.Product[]
            {
                new Model.Product
                {
                    ProductNumber = "FAC-00011",
                    Description = "Standard long term loan"
                }
            });
    }
}
