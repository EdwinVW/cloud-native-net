namespace OutputManagementService.Repositories;

using ContractManagement.Domain.Aggregates.Contract.Enums;

public interface IContractRepository
{
    Task<Contract?> GetContractByContractNumberAsync(string contractNumber);
    
    Task AddContractAsync(Contract contract);
    
    Task UpdateContractAsync(Contract contract);
    
    Task MarkContractAsSentAsync(string contractNumber);
    
    Task<IEnumerable<Contract>> GetContractsToSendAsync(DateOnly startDate);
    
    Task DeleteContract(string contractNumber);
}
