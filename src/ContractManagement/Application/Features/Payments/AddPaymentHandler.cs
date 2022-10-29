namespace Contractmanagement.Features.Payments;

public class AddPaymentHandler : ICommandHandler<AddPayment>
{
    private readonly IAggregateService<Account> _aggregateService;

    public AddPaymentHandler(IAggregateService<Account> aggregateService)
    {
        _aggregateService = aggregateService;
    }

    public async ValueTask HandleAsync(AddPayment command)
    {
        var account = await _aggregateService.RehydrateAsync(
            command.AccountNumber, command.ExpectedVersion);

        account.AddPayment(command);
        
        await _aggregateService.ProcessChangesAsync(account);
    }
}
