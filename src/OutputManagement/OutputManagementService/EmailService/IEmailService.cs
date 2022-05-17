namespace OutputManagementService.EmailService;

public interface IEmailService
{
    Task SendContract(ContractInfo contractInfo);
}
