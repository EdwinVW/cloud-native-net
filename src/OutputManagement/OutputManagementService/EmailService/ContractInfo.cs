namespace OutputManagementService.EmailService
{
    public record ContractInfo(
        string ContractNumber,
        string ProductNumber,
        string ProductDescription,
        string CustomerEmail);
}