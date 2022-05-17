using ContractManagement.Application.IntegrationEvents;

namespace OutputManagementService.Consumers;

public class OutputManagement_ContractTermChangedConsumer : MassTransitConsumer<ContractTermChanged>
{
    private readonly IContractRepository _contractRepository;

    public OutputManagement_ContractTermChangedConsumer(
        ILogger<OutputManagement_ContractTermChangedConsumer> logger,
        IContractRepository contractRepository) : base(logger)
    {
        this._contractRepository = contractRepository;
    }

    protected override async Task ConsumeMessage(ContractTermChanged message)
    {
        var existingContract = await _contractRepository.GetContractByContractNumberAsync(message.ContractNumber);
        if (existingContract != null)
        {
            await _contractRepository.UpdateContractAsync(
                existingContract with
                {
                    StartDate = message.StartDate,
                    EndDate = message.EndDate,
                    ContractSent = false
                });
        }
        else
        {
            _logger.LogInformation("Contract {contractNumber} not found.", message.ContractNumber);
        }
    }
}
