namespace Contractmanagement.Features.ContractMaintenance;

using DomainEvents = ContractManagement.Domain.Aggregates.Contract.DomainEvents;
using IntegrationEvents = ContractManagement.Application.IntegrationEvents;

public class ChangeContractTermFeature : 
    ICommandHandler<ChangeContractTerm>, 
    IEventHandler<ContractTermChanged>,
    IProjection<ContractTermChanged>
{
    private readonly IAggregateService<EventSourcedEntityId, Contract> _aggregateService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IContractReadModelRepository _contractReadModelRepository;

    public ChangeContractTermFeature(
        IAggregateService<EventSourcedEntityId, Contract> aggregateService,
        IUnitOfWork unitOfWork, 
        IContractReadModelRepository contractReadModelRepository)
    {
        _aggregateService = aggregateService;
        _unitOfWork = unitOfWork;
        _contractReadModelRepository = contractReadModelRepository;        
    }

    public async ValueTask HandleAsync(ChangeContractTerm command)
    {
        var contract = await _aggregateService.RehydrateAsync(command.ContractNumber, command.ExpectedVersion);
        await contract.ChangeContractTerm(command);
        await _aggregateService.ProcessChangesAsync(contract);
    }    

    public async ValueTask HandleAsync(DomainEvents.ContractTermChanged domainEvent)
    {
        var contract = await _contractReadModelRepository.GetContractByContractNumberAsync(domainEvent.ContractNumber);
        var integrationEvent = IntegrationEvents.ContractTermChanged.CreateFrom(domainEvent);
        _unitOfWork.AddIntegrationEventToPublish(integrationEvent);
    }    

    public async ValueTask ProjectAsync(ContractTermChanged domainEvent)
    {
        var contract = await _contractReadModelRepository.GetContractByContractNumberAsync(domainEvent.ContractNumber);
        if (contract != null)
        {
            contract.StartDate = domainEvent.StartDate;
            contract.EndDate = domainEvent.EndDate;
        }
    }    
}