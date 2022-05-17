namespace ContractManagement.Application.Interfaces;

public interface IContractReadModelRepository
{
    Task AddContractAsync(ReadModels.Contract contract);

    ValueTask<ReadModels.Contract> GetContractByContractNumberAsync(string contractNumber);

    Task DeleteContractAsync(string contractNumber);
}