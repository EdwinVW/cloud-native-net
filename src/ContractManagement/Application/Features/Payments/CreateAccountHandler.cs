namespace Contractmanagement.Features.Payments;

public class CreateAccountHandler : ICommandHandler<CreateAccount>
{
    private readonly IAggregateService<Account> _aggregateService;

    public CreateAccountHandler(IAggregateService<Account> aggregateService)
    {
        _aggregateService = aggregateService;
    }

    public async ValueTask HandleAsync(CreateAccount command)
    {
        var account = new Account();

        account.CreateAccount(command);
        
        await _aggregateService.ProcessChangesAsync(account);
    }
}
