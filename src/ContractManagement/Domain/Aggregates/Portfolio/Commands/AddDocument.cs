namespace ContractManagement.Domain.Aggregates.Portfolio.Commands;

public record AddDocument(string PortfolioId, string DocumentId, string documentType, string DocumentURL) : Command;
