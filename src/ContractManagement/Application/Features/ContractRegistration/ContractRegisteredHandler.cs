namespace Contractmanagement.Features.ContractRegistration;

using DomainEvents = ContractManagement.Domain.Aggregates.Contract.DomainEvents;
using IntegrationEvents = ContractManagement.Application.IntegrationEvents;

public class ContractRegisteredHandler : IEventHandler<DomainEvents.ContractRegisteredV2>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICommandHandler<CreatePortfolio> _createPortfolioHandler;

    public ContractRegisteredHandler(IUnitOfWork unitOfWork, ICommandHandler<CreatePortfolio> createPortfolioHandler)
    {
        this._createPortfolioHandler = createPortfolioHandler;
        _unitOfWork = unitOfWork;
    }

    public async ValueTask HandleAsync(DomainEvents.ContractRegisteredV2 domainEvent)
    {
        // create new document portfolio for this contract
        var command = new CreatePortfolio(domainEvent.ContractNumber);
        await _createPortfolioHandler.HandleAsync(command);

        // send integration event
        var integrationEvent = IntegrationEvents.ContractRegistered.CreateFrom(domainEvent);
        _unitOfWork.AddIntegrationEventToPublish(integrationEvent);
    }
}
