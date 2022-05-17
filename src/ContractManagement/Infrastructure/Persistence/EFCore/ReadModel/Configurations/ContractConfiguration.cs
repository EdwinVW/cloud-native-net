using ContractManagement.Domain.Aggregates.ContractAggregate.Enums;
using Model = ContractManagement.Application.ReadModels;

namespace ContractManagement.Infrastructure.Persistence.EFCore.ReadModel.Configurations;

public class ContractConfiguration : IEntityTypeConfiguration<Model.Contract>
{
    public void Configure(EntityTypeBuilder<Model.Contract> builder)
    {
        builder.ToTable("Contract")
            .HasKey(e => e.ContractNumber);

        builder
            .Property(e => e.ContractNumber)
            .IsFixedLength()
            .HasMaxLength(18);

        builder
            .Property(e => e.CustomerNumber)
            .IsFixedLength()
            .HasMaxLength(6);

        builder
            .Property(e => e.Amount)
            .HasPrecision(10, 2);

        builder
            .Property(e => e.PaymentPeriod)
            .HasConversion<string>()
            .HasMaxLength(25);

        builder
            .HasData(new Model.Contract[]
            {
                new Model.Contract
                {
                    ContractNumber = "CTR-20220502-9999",
                    CustomerNumber = "C13976",
                    ProductNumber = "FAC-00011",
                    Amount = 20000,
                    StartDate = DateTime.Parse("2022-05-02T12:40:35.876Z"),
                    EndDate = DateTime.Parse("2034-05-02T12:40:35.877Z"),
                    PaymentPeriod = PaymentPeriod.Monthly
                }
            });
    }
}
