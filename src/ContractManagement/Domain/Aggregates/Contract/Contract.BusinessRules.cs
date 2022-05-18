namespace ContractManagement.Domain.Aggregates.ContractAggregate;

public partial class Contract
{
    private async Task EnsureExistingProduct(string productNumber, IProductService productService)
    {
        if (!await productService.IsExistingProductAsync(productNumber))
        {
            AddBusinessRuleViolation($"Product with product-number {productNumber} not found.");
        }
    }

    private async Task EnsureExistingCustomer(string customerNumber, ICustomerService customerService)
    {
        if (!await customerService.IsExistingCustomerAsync(customerNumber))
        {
            AddBusinessRuleViolation($"Customer with customer-number {customerNumber} not found.");
        }
    }

    private void EnsureNotCancelled()
    {
        if (Cancelled)
        {
            AddBusinessRuleViolation("The contract was cancelled.");
        }
    }

    private void EnsureValidAmount(decimal amount)
    {
        // Contract amount must be between 1000 and 10000000
        if (amount < 1000 || amount > 10000000)
        {
            AddBusinessRuleViolation(
                "Invalid amount. The amount on a contract must be between 1.000 and 10.000.000 Euros.");
        }
    }

    private void EnsureValidTerm(DateTime startDate, DateTime endDate)
    {
        // Contract term must be at least 5 years
        if (endDate < startDate.Date.AddYears(5))
        {
            AddBusinessRuleViolation(
                "Invalid contract term. The term should be at least 5 years.");
        }

        // Contract term must be no longer than 50 years
        if (endDate > startDate.Date.AddYears(50))
        {
            AddBusinessRuleViolation(
                "Invalid contract term. The term should be no longer than 50 years.");
        }
    }

    private void EnsureValidTermForCancellation()
    {
        if (DateTime.Now.Date >= ContractTerm?.EndDate.Date.AddYears(-3))
        {
            AddBusinessRuleViolation("Contract can not be cancelled if it is within 3 years from the end of its term.");
        }
    }
}