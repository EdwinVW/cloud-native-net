namespace ContractManagement.Domain.Aggregates.Contract.DomainEvents;

public record ContractTermChanged(
    string ContractNumber,
    DateTime StartDate,
    DateTime EndDate) : Event
{
    public static ContractTermChanged CreateFrom(ChangeContractTerm command) =>
        new ContractTermChanged(
            command.ContractNumber,
            command.StartDate,
            command.EndDate);
}