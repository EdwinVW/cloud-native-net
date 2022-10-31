namespace ContractManagement.Domain.Aggregates.Portfolio;

public class Document : Entity
{
    public override string Id => DocumentId;

    public string DocumentId { get; set; } = string.Empty;

    public string PortfolioId { get; set; } = string.Empty;

    public DocumentType DocumentType { get; set; } = DocumentType.Other;

    public string DocumentUrl { get; set; } = string.Empty;

    public static Document Create(string portfolioId, string documentId, DocumentType documentType, string documentUrl)
    {
        return new Document
        {
            DocumentId = documentId,
            PortfolioId = portfolioId,
            DocumentType = documentType,
            DocumentUrl = documentUrl
        };
    }
}
