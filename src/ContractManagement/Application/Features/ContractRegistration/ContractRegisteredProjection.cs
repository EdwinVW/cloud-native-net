namespace Contractmanagement.Features.ContractRegistration;

using ReadModels = ContractManagement.Application.ReadModels;

public class ContractRegisteredProjection : 
    IProjection<ContractRegistered>, 
    IProjection<ContractRegisteredV2>
{
    private readonly IContractReadModelRepository _readModelRepository;

    public ContractRegisteredProjection(IContractReadModelRepository readModelRepository)
    {
        _readModelRepository = readModelRepository;
    }

    public async ValueTask ProjectAsync(ContractRegistered domainEvent)
    {
        var contractRegisteredV2 = ContractRegisteredV2.CreateFrom(domainEvent);
        await ProjectAsync(contractRegisteredV2);
    }

    public async ValueTask ProjectAsync(ContractRegisteredV2 domainEvent)
    {
        await _readModelRepository.AddContractAsync(
            new ReadModels.Contract
            {
                ContractNumber = domainEvent.ContractNumber,
                CustomerNumber = domainEvent.CustomerNumber,
                ProductNumber = domainEvent.ProductNumber,
                Amount = domainEvent.Amount,
                StartDate = domainEvent.StartDate,
                EndDate = domainEvent.EndDate,
                PaymentPeriod = domainEvent.PaymentPeriod
            });
    }
  
}
