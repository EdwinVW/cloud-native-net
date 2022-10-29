namespace Contractmanagement.Features.ContractRegistration;

using DomainEvents = ContractManagement.Domain.Aggregates.Contract.DomainEvents;
using IntegrationEvents = ContractManagement.Application.IntegrationEvents;

public class ContractRegisteredHandler : IEventHandler<DomainEvents.ContractRegisteredV2>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICommandHandler<CreateAccount> _createAccountHandler;

    public ContractRegisteredHandler(IUnitOfWork unitOfWork, ICommandHandler<CreateAccount> createAccountHandler)
    {
        this._createAccountHandler = createAccountHandler;
        _unitOfWork = unitOfWork;
    }

    public async ValueTask HandleAsync(DomainEvents.ContractRegisteredV2 domainEvent)
    {
        // create new account for this contract
        var createAccount = new CreateAccount(domainEvent.ContractNumber);
        await _createAccountHandler.HandleAsync(createAccount);

        // send integration event
        var integrationEvent = IntegrationEvents.ContractRegistered.CreateFrom(domainEvent);
        _unitOfWork.AddIntegrationEventToPublish(integrationEvent);
    }
}
