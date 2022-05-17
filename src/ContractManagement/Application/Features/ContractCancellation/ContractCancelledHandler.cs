namespace Contractmanagement.Features.ContractCancellation;

using DomainEvents = ContractManagement.Domain.Aggregates.ContractAggregate.DomainEvents;
using IntegrationEvents = ContractManagement.Application.IntegrationEvents;

public class ContractCancelledHandler : IEventHandler<DomainEvents.ContractCancelled>
{
    private readonly IUnitOfWork _unitOfWork;

    public ContractCancelledHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public ValueTask HandleAsync(DomainEvents.ContractCancelled domainEvent)
    {
        var integrationEvent = IntegrationEvents.ContractCancelled.CreateFrom(domainEvent);

        _unitOfWork.AddIntegrationEventToPublish(integrationEvent);
        
        return ValueTask.CompletedTask;
    }
}
