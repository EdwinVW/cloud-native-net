using ContractManagement.Application.IntegrationEvents;

namespace OutputManagementService.Consumers;

public class OutputManagement_ContractAmountChangedConsumer : MassTransitConsumer<ContractAmountChanged>
{
    private readonly IContractRepository _contractRepository;

    public OutputManagement_ContractAmountChangedConsumer(
        ILogger<OutputManagement_ContractAmountChangedConsumer> logger,
        IContractRepository contractRepository) : base(logger)
    {
        this._contractRepository = contractRepository;
    }

    protected override async Task ConsumeMessage(ContractAmountChanged message)
    {
        var existingContract = await _contractRepository.GetContractByContractNumberAsync(message.ContractNumber);
        if (existingContract != null)
        {
            await _contractRepository.UpdateContractAsync(
                existingContract with
                {
                    Amount = message.NewAmount,
                    ContractSent = false
                });
        }
        else
        {
            _logger.LogInformation("Contract {contractNumber} not found.", message.ContractNumber);
        }
    }
}
