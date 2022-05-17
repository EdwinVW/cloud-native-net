namespace Contractmanagement.Features.ContractRegistration;

using DomainEvents = ContractManagement.Domain.Aggregates.ContractAggregate.DomainEvents;
using IntegrationEvents = ContractManagement.Application.IntegrationEvents;

public class ContractRegisteredHandler : IEventHandler<DomainEvents.ContractRegisteredV2>
{
    private readonly IUnitOfWork _unitOfWork;

    public ContractRegisteredHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public ValueTask HandleAsync(DomainEvents.ContractRegisteredV2 domainEvent)
    {
        var integrationEvent = IntegrationEvents.ContractRegistered.CreateFrom(domainEvent);
        _unitOfWork.AddIntegrationEventToPublish(integrationEvent);
        return ValueTask.CompletedTask;
    }
}
