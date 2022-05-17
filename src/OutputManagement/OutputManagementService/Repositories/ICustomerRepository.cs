namespace OutputManagementService.Repositories
{
    public interface ICustomerRepository
    {
        Task AddCustomerAsync(Customer customer);
        Task<Customer?> GetCustomerByCustomerNumberAsync(string customerNumber);
    }
}