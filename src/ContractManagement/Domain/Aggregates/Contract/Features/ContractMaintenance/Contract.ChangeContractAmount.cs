namespace ContractManagement.Domain.Aggregates.ContractAggregate
{
    public partial class Contract
    {
        public ValueTask ChangeContractAmount(ChangeContractAmount command)
        {
            EnsureNotCancelled();
            EnsureValidAmount(command.NewAmount);
            if (IsValid)
            {
                var contractAmountChanged = ContractAmountChanged.CreateFrom(command);
                ApplyDomainEvent(contractAmountChanged);
            }
            return ValueTask.CompletedTask;
        }

        private void Handle(ContractAmountChanged domainEvent)
        {
            Amount = MoneyAmount.Parse(domainEvent.NewAmount);
        }
    }
}