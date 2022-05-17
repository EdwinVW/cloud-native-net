namespace Contractmanagement.Features.ContractMaintenance;

using DomainEvents = ContractManagement.Domain.Aggregates.ContractAggregate.DomainEvents;
using IntegrationEvents = ContractManagement.Application.IntegrationEvents;

public class ChangeContractAmountFeature : 
    ICommandHandler<ChangeContractAmount>, 
    IEventHandler<DomainEvents.ContractAmountChanged>,
    IProjection<ContractAmountChanged>
{
    private readonly IAggregateService<EventSourcedEntityId, Contract> _aggregateService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IContractReadModelRepository _contractReadModelRepository;

    public ChangeContractAmountFeature(
        IAggregateService<EventSourcedEntityId, Contract> aggregateService,
        IUnitOfWork unitOfWork, 
        IContractReadModelRepository contractReadModelRepository)
    {
        _aggregateService = aggregateService;
        _unitOfWork = unitOfWork;
        _contractReadModelRepository = contractReadModelRepository;        
    }

    public async ValueTask HandleAsync(ChangeContractAmount command)
    {
        var contract = await _aggregateService.RehydrateAsync(command.ContractNumber, command.ExpectedVersion);
        await contract.ChangeContractAmount(command);
        await _aggregateService.ProcessChangesAsync(contract);
    }    

    public async ValueTask HandleAsync(DomainEvents.ContractAmountChanged domainEvent)
    {
        var contract = await _contractReadModelRepository.GetContractByContractNumberAsync(domainEvent.ContractNumber);
        var integrationEvent = IntegrationEvents.ContractAmountChanged.CreateFrom(domainEvent);
        _unitOfWork.AddIntegrationEventToPublish(integrationEvent);
    }    

    public async ValueTask ProjectAsync(ContractAmountChanged domainEvent)
    {
        var contract = await _contractReadModelRepository.GetContractByContractNumberAsync(domainEvent.ContractNumber);
        if (contract != null)
        {
            contract.Amount = domainEvent.NewAmount;
        }
    }    
}