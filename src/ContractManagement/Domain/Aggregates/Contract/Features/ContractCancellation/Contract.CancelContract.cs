namespace ContractManagement.Domain.Aggregates.ContractAggregate
{
    public partial class Contract
    {
        public ValueTask CancelContract(CancelContract command)
        {
            EnsureNotCancelled();
            EnsureValidTermForCancellation();
            if (IsValid)
            {
                var contractCancelled = ContractCancelled.CreateFrom(command);
                ApplyDomainEvent(contractCancelled);
            }
            return ValueTask.CompletedTask;
        }

        private void Handle(ContractCancelled domainEvent)
        {
            Cancelled = true;
        }
    }
}