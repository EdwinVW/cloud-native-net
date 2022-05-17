using ContractManagement.Domain.Aggregates.ContractAggregate.Enums;

namespace OutputManagementService.Repositories
{
    public class InMemoryContractRepository : IContractRepository
    {
        private readonly ILogger<InMemoryContractRepository> _logger;

        public InMemoryContractRepository(ILogger<InMemoryContractRepository> logger)
        {
            this._logger = logger;

        }
        private IList<Contract> _contracts = new List<Contract>
        {
            new Contract(
                "CTR-20220502-9999",
                "C13976",
                "FAC-00011",
                20000,
                DateTime.Parse("2022-05-02T12:40:35.876Z"),
                DateTime.Parse("2034-05-02T12:40:35.877Z"),
                PaymentPeriod.Monthly,
                false)
        };

        public Task<Contract?> GetContractByContractNumberAsync(string contractNumber)
        {
            return Task.FromResult(_contracts.FirstOrDefault(c => c.ContractNumber == contractNumber));
        }

        public Task AddContractAsync(Contract contract)
        {
            var existingContract = _contracts.FirstOrDefault(c => c.ContractNumber == contract.ContractNumber);
            if (existingContract != null)
            {
                _contracts.Remove(existingContract);
            }
            _contracts.Add(contract);

            _logger.LogInformation("Added contract {contractNumber} for customer {customerNumber}.",
                contract.ContractNumber, contract.CustomerNumber);

            return Task.CompletedTask;
        }

        public Task UpdateContractAsync(Contract contract)
        {
            var existingContract = _contracts.FirstOrDefault(c => c.ContractNumber == contract.ContractNumber);
            if (existingContract != null)
            {
                _contracts.Remove(existingContract);
                _contracts.Add(existingContract with
                {
                    CustomerNumber = contract.CustomerNumber,
                    ProductNumber = contract.ProductNumber,
                    Amount = contract.Amount,
                    StartDate = contract.StartDate,
                    EndDate = contract.EndDate,
                    Paymentperiod = contract.Paymentperiod,
                    ContractSent = contract.ContractSent
                });

                _logger.LogInformation("Updated contract {contractNumber}.", contract.ContractNumber);
            }
            else
            {
                _logger.LogError("Contract {contractNumber} not found.", contract.ContractNumber);
            }

            return Task.CompletedTask;
        }

        public Task<IEnumerable<Contract>> GetContractsToSendAsync(DateOnly startDate)
        {
            return Task.FromResult(_contracts.Where(c =>
                DateOnly.FromDateTime(c.StartDate) == startDate && c.ContractSent == false));
        }

        public Task MarkContractAsSentAsync(string contractNumber)
        {
            var existingContract = _contracts.FirstOrDefault(c => c.ContractNumber == contractNumber);
            if (existingContract != null)
            {
                _contracts.Remove(existingContract);
                _contracts.Add(existingContract with { ContractSent = true });
                _logger.LogInformation("Marked contract {contractNumber} as sent.", contractNumber);
            }
            else
            {
                _logger.LogError("Contract {contractNumber} not found.", contractNumber);
            }

            return Task.CompletedTask;
        }

        public Task DeleteContract(string contractNumber)
        {
            var existingContract = _contracts.FirstOrDefault(c => c.ContractNumber == contractNumber);
            if (existingContract != null)
            {
                _contracts.Remove(existingContract);
                _logger.LogInformation("Deleted cancelled contract {contractNumber}.", contractNumber);
            }
            else
            {
                _logger.LogError("Contract {contractNumber} not found.", contractNumber);
            }
            return Task.CompletedTask;
        }
    }
}