namespace ContractManagement.Infrastructure.Persistence.EFCore.Configurations;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable("Document")
            .HasKey(e => e.DocumentId);

        builder
            .Property(e => e.DocumentType)
            .HasMaxLength(50)
            .HasConversion<string>();

        builder
            .Property(e => e.DocumentUrl)
            .HasMaxLength(256);

        builder
            .HasData(new Document[]
            {
                new Document
                {
                    DocumentId = "F656C97A-B211-4618-A39F-E14C6CB2D003",
                    PortfolioId = "CTR-20220502-9999",
                    DocumentType = DocumentType.Passport,
                    DocumentUrl = "file://archivesrv01/contracts/CTR-20220502-9999/F656C97A-B211-4618-A39F-E14C6CB2D003.pdf"
                }
            });
    }
}
