namespace ContractManagement.Domain.Aggregates.Portfolio;

public class Portfolio : AggregateRoot
{
    public override string Id => PortfolioId;

    public string PortfolioId { get; set; } = string.Empty;

    public List<Document> Documents { get; set; } = new List<Document>();

    public void CreatePortfolio(CreatePortfolio command)
    {
        PortfolioId = command.PortfolioId;
    }

    public void AddDocument(AddDocument command)
    {
        DocumentType documentType = DocumentType.Other;

        if (command.DocumentId == null)
        {
            AddBusinessRuleViolation("A document Id is mandatory.");
            return;
        }
        if (command.DocumentURL == null)
        {
            AddBusinessRuleViolation("A document URL is mandatory.");
            return;
        }
        if (!Enum.TryParse<DocumentType>(command.documentType, out documentType))
        {
            AddBusinessRuleViolation("Invalid document type specified.");
            return;
        }

        Documents.Add(Document.Create(
            command.PortfolioId, command.DocumentId, documentType, command.DocumentURL));
    }
}
