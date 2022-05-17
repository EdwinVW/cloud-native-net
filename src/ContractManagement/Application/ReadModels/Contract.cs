namespace ContractManagement.Application.ReadModels;

public class Contract
{
    public string? ContractNumber { get; set; }

    public string? CustomerNumber { get; set; }
    
    public string? ProductNumber { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public PaymentPeriod PaymentPeriod { get; set; }
}