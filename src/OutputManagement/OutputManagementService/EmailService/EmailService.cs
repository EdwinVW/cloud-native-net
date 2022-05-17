namespace OutputManagementService.EmailService;

public class EmailService : IEmailService
{
    private readonly ILogger _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        this._logger = logger;

    }
    public Task SendContract(ContractInfo contractInfo)
    {
        _logger.LogInformation("Sending contract {contractNumber} for {product} to {emailAddress}.", 
            contractInfo.ContractNumber, contractInfo.ProductNumber, contractInfo.CustomerEmail);
        return Task.CompletedTask;
    }
}
