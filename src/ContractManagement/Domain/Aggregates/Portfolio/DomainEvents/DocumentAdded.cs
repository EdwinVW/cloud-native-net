namespace ContractManagement.Domain.Aggregates.Portfolio.DomainEvents;

public record DocumentAdded(string PortfolioId, string DocumentId, string documentType, string DocumentURL) : Event
{
    public static DocumentAdded CeateFrom(AddDocument command) =>
        new DocumentAdded(command.PortfolioId, command.DocumentId, command.documentType, command.DocumentURL);
}