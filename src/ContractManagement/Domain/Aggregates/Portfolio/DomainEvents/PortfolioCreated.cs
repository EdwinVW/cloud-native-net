namespace ContractManagement.Domain.Aggregates.Portfolio.DomainEvents;

public record PortfolioCreated(string PortfolioId) : Event
{
    public static PortfolioCreated CreateFrom(CreatePortfolio command) =>
        new PortfolioCreated(command.PortfolioId);
}