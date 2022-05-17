using Model = ContractManagement.Application.ReadModels;

namespace ContractManagement.Infrastructure.Persistence.EFCore.Repositories;

public class EFContractReadModelRepository : IContractReadModelRepository
{
    private readonly ServiceDbContext _dbContext;
    private readonly ILogger<EFContractReadModelRepository> _logger;

    public EFContractReadModelRepository(ILogger<EFContractReadModelRepository> logger, ServiceDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task AddContractAsync(Model.Contract contract)
    {
        await _dbContext.AddAsync(contract);

        _logger.LogInformation("Added contract {contractNumber}.", contract.ContractNumber);
    }

    public async ValueTask<Model.Contract> GetContractByContractNumberAsync(string contractNumber)
    {
        return await _dbContext.Contracts.FirstOrDefaultAsync(c => c.ContractNumber == contractNumber);
    }

    public async Task DeleteContractAsync(string contractNumber)
    {
        var contract = await _dbContext.Contracts.FirstOrDefaultAsync(c => c.ContractNumber == contractNumber);
        if (contract != null)
        {
            _dbContext.Contracts.Remove(contract!);
        }

        _logger.LogInformation("Deleted cancelled contract {contractNumber}.", contractNumber);
    }
}
