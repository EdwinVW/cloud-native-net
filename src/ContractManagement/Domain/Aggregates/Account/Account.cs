namespace ContractManagement.Domain.Aggregates.Account;

public class Account : AggregateRoot
{
    public override string Id => AccountNumber;

    public string AccountNumber { get; set; }

    public decimal Balance { get; set; }

    public Account()
    {
        AccountNumber = string.Empty;
        Balance = 0;
    }

    public void CreateAccount(CreateAccount command)
    {
        AccountNumber = command.AccountNumber;
    }    

    public void AddPayment(AddPayment command)
    {
        if (command.Amount < 0)
        {
            AddBusinessRuleViolation("A payment amount cannot be negative.");
            return;
        }
        Balance += command.Amount;
    }
}
