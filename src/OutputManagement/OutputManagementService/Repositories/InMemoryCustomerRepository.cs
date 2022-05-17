namespace OutputManagementService.Repositories
{
    public class InMemoryCustomerRepository : ICustomerRepository
    {
        private readonly ILogger<InMemoryCustomerRepository> _logger;

        public InMemoryCustomerRepository(ILogger<InMemoryCustomerRepository> logger)
        {
            this._logger = logger;

        }
        private IList<Customer> _customers = new List<Customer>
        {
            new Customer("C13976", "jd@example.com"),
            new Customer("C13977", "eric.dewitt@example.com")
        };

        public Task AddCustomerAsync(Customer customer)
        {
            var existingCustomer = _customers.FirstOrDefault(c => c.CustomerNumber == customer.CustomerNumber);
            if (existingCustomer != null)
            {
                _customers.Remove(existingCustomer);
            }
            _customers.Add(customer);

            _logger.LogInformation("Added customer {customerNumber}.", customer.CustomerNumber);
            
            return Task.CompletedTask;
        }

        public Task<Customer?> GetCustomerByCustomerNumberAsync(string customerNumber)
        {
            return Task.FromResult(_customers.FirstOrDefault(c => c.CustomerNumber == customerNumber));
        }
    }
}