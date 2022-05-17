using ContractManagement.Application.IntegrationEvents;

namespace OutputManagementService.Consumers;

public class OutputManagement_ContractRegisteredConsumer : MassTransitConsumer<ContractRegistered>
{
    private readonly IContractRepository _contractRepository;

    public OutputManagement_ContractRegisteredConsumer(
        ILogger<OutputManagement_ContractRegisteredConsumer> logger,
        IContractRepository contractRepository) : base(logger)
    {
        this._contractRepository = contractRepository;
    }

    protected override async Task ConsumeMessage(ContractRegistered message)
    {
        var contractRegistered = message;
        await _contractRepository.AddContractAsync(
            new Contract(
                contractRegistered.ContractNumber,
                contractRegistered.CustomerNumber,
                contractRegistered.ProductNumber,
                contractRegistered.Amount,
                contractRegistered.StartDate,
                contractRegistered.EndDate,
                contractRegistered.PaymentPeriod,
                false));
    }
}
