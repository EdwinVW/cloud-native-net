using ContractManagement.Application.IntegrationEvents;

namespace OutputManagementService.Consumers;

public class OutputManagement_ContractCancelledConsumer : MassTransitConsumer<ContractCancelled>
{
    private readonly IContractRepository _contractRepository;

    public OutputManagement_ContractCancelledConsumer(
        ILogger<OutputManagement_ContractCancelledConsumer> logger,
        IContractRepository contractRepository) : base(logger)
    {
        this._contractRepository = contractRepository;
    }

    protected override async Task ConsumeMessage(ContractCancelled message)
    {
        var existingContract = await _contractRepository.GetContractByContractNumberAsync(message.ContractNumber);
        if (existingContract != null)
        {
            await _contractRepository.DeleteContract(message.ContractNumber);
        }
        else
        {
            _logger.LogInformation("Contract {contractNumber} not found.", message.ContractNumber);
        }
    }
}
