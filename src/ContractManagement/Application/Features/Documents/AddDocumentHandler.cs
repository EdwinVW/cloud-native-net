namespace Contractmanagement.Features.Documents;

public class AddDocumentHandler : ICommandHandler<AddDocument>
{
    private readonly IAggregateService<Portfolio> _aggregateService;

    public AddDocumentHandler(IAggregateService<Portfolio> aggregateService)
    {
        _aggregateService = aggregateService;
    }

    public async ValueTask HandleAsync(AddDocument command)
    {
        var portFolio = await _aggregateService.RehydrateAsync(
            command.PortfolioId, command.ExpectedVersion);

        portFolio.AddDocument(command);
        
        await _aggregateService.ProcessChangesAsync(portFolio);
    }
}
