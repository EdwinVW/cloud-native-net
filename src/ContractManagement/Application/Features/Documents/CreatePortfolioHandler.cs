namespace Contractmanagement.Features.Documents;

public class CreatePortfolioHandler : ICommandHandler<CreatePortfolio>
{
    private readonly IAggregateService<Portfolio> _aggregateService;

    public CreatePortfolioHandler(IAggregateService<Portfolio> aggregateService)
    {
        _aggregateService = aggregateService;
    }

    public async ValueTask HandleAsync(CreatePortfolio command)
    {
        var portfolio = new Portfolio();

        portfolio.CreatePortfolio(command);
        
        await _aggregateService.ProcessChangesAsync(portfolio);
    }
}
