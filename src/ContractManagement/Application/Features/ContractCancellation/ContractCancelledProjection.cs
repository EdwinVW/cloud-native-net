namespace Contractmanagement.Features.ContractCancellation;

public class ContractCancelledProjection : IProjection<ContractCancelled>
{
    private readonly IContractReadModelRepository _readModelRepository;

    public ContractCancelledProjection(IContractReadModelRepository readModelRepository)
    {
        _readModelRepository = readModelRepository;
    }

    public async ValueTask ProjectAsync(ContractCancelled domainEvent)
    {
        await _readModelRepository.DeleteContractAsync(domainEvent.ContractNumber);
    }
}
