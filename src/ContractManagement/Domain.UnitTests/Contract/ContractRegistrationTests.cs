namespace Domain.UnitTests.ContractAggregate;

[TestClass]
public class ContractRegistrationTests
{
    [TestMethod]
    public async Task RegisterContract_WithCorrectValues_ShouldYieldValidContract()
    {
        // Arrange
        string aggregateId = "CTR-20220424-0001";
        var command = RegisterContractV2Builder.Build(aggregateId);
        var customerServiceMock = CustomerServiceMock.ForExistingCustomer(command.CustomerNumber);
        var productServiceMock = ProductServiceMock.ForExistingProduct(command.ProductNumber);
        var sut = new Contract(aggregateId);

        // Act
        await sut.RegisterContractAsync(
            command, 
            customerServiceMock.Object, 
            productServiceMock.Object);


        // Assert
        sut.IsValid.Should().BeTrue();
        sut.ContractNumber.Value.Should().BeEquivalentTo(command.ContractNumber);
        sut.CustomerNumber!.Value.Should().BeEquivalentTo(command.CustomerNumber);
        sut.ProductNumber!.Value.Should().BeEquivalentTo(command.ProductNumber);
        sut.Amount!.Value.Should().Be(command.Amount);
        sut.ContractTerm!.StartDate.Should().BeSameDateAs(command.StartDate);
        sut.ContractTerm.EndDate.Should().BeSameDateAs(command.EndDate);
        sut.PaymentPeriod.Should().Be(command.PaymentPeriod);
        sut.Cancelled.Should().BeFalse();

        sut.GetDomainEvents().Should().ContainSingle(e => e is ContractRegisteredV2)
            .Which.Should().BeEquivalentTo(command, options => options
                .ExcludingMissingMembers()
                .Excluding(e => e.Type));
    }

    [TestMethod]
    public void Rehydrate_ContractRegistered_ShouldYieldValidContract()
    {
        // Arrange
        string aggregateId = "CTR-20220424-0001";
        var domainEvent = ContractRegisteredBuilder.Build(aggregateId);

        // Act
        var sut = new Contract(
            aggregateId, 
            new List<Event> { domainEvent });

        // Assert
        sut.IsValid.Should().BeTrue();
        sut.ContractNumber.Value.Should().BeEquivalentTo(domainEvent.ContractNumber);
        sut.CustomerNumber!.Value.Should().BeEquivalentTo(domainEvent.CustomerNumber);
        sut.ProductNumber!.Value.Should().BeEquivalentTo(domainEvent.ProductNumber);
        sut.Amount!.Value.Should().Be(domainEvent.Amount);
        sut.ContractTerm!.StartDate.Should().BeSameDateAs(domainEvent.StartDate);
        sut.ContractTerm.EndDate.Should().BeSameDateAs(domainEvent.EndDate);
        // Monthly is used the default PaymentPeriod in the conversion from 
        // a ContractRegistered to a ContractRegisteredV2 event 
        sut.PaymentPeriod.Should().Be(PaymentPeriod.Monthly);
        sut.Cancelled.Should().BeFalse();

        sut.GetDomainEvents().Should().BeEmpty();        
    }

    [TestMethod]
    public void Rehydrate_ContractRegisteredV2_ShouldYieldValidContract()
    {
        // Arrange
        string aggregateId = "CTR-20220424-0001";
        var domainEvent = ContractRegisteredV2Builder.Build(aggregateId)
            with
        { PaymentPeriod = PaymentPeriod.Quarterly };

        // Act
        var sut = new Contract(aggregateId, new List<Event> { domainEvent });

        // Assert
        sut.IsValid.Should().BeTrue();
        sut.ContractNumber.Value.Should().BeEquivalentTo(domainEvent.ContractNumber);
        sut.CustomerNumber!.Value.Should().BeEquivalentTo(domainEvent.CustomerNumber);
        sut.ProductNumber!.Value.Should().BeEquivalentTo(domainEvent.ProductNumber);
        sut.Amount!.Value.Should().Be(domainEvent.Amount);
        sut.ContractTerm!.StartDate.Should().BeSameDateAs(domainEvent.StartDate);
        sut.ContractTerm.EndDate.Should().BeSameDateAs(domainEvent.EndDate);
        sut.PaymentPeriod.Should().Be(domainEvent.PaymentPeriod);
        sut.Cancelled.Should().BeFalse();

        sut.GetDomainEvents().Should().BeEmpty();
    }

    [TestMethod]
    public async Task RegisterContract_WithNonExistingCustomer_ShouldYieldViolation()
    {
        // Arrange
        string aggregateId = "CTR-20220424-0001";
        var command = RegisterContractV2Builder.Build(aggregateId);
        var customerServiceMock = CustomerServiceMock.ForNonExistingCustomer(command.CustomerNumber);
        var productServiceMock = ProductServiceMock.ForExistingProduct(command.ProductNumber);
        var sut = new Contract(aggregateId);

        // Act
        await sut.RegisterContractAsync(
            command, 
            customerServiceMock.Object, 
            productServiceMock.Object);

        // Assert
        sut.IsValid.Should().BeFalse();
        sut.GetBusinessRuleViolations().Should().ContainSingle(
            $"Customer with customer-number {command.CustomerNumber} not found.");
    }

    [TestMethod]
    public async Task RegisterContract_WithNonExistingProduct_ShouldYieldViolation()
    {
        // Arrange
        string aggregateId = "CTR-20220424-0001";
        var command = RegisterContractV2Builder.Build(aggregateId);
        var customerServiceMock = CustomerServiceMock.ForExistingCustomer(command.CustomerNumber);
        var productServiceMock = ProductServiceMock.ForNonExistingProduct(command.ProductNumber);
        var sut = new Contract(aggregateId);

        // Act
        await sut.RegisterContractAsync(
            command, 
            customerServiceMock.Object, 
            productServiceMock.Object);

        // Assert
        sut.IsValid.Should().BeFalse();
        sut.GetBusinessRuleViolations().Should().ContainSingle(
            $"Product with product-number {command.ProductNumber} not found.");
    }     

    [TestMethod]
    public async Task RegisterContract_WithTooShortContractTerm_ShouldYieldViolation()
    {
        // Arrange
        string aggregateId = "CTR-20220424-0001";
        var command = RegisterContractV2Builder.Build(aggregateId)
            with { EndDate = new DateTime(2025, 4, 24, 18, 33, 5) };
        var customerServiceMock = CustomerServiceMock.ForExistingCustomer(command.CustomerNumber);
        var productServiceMock = ProductServiceMock.ForExistingProduct(command.ProductNumber);
        var sut = new Contract(aggregateId);

        // Act
        await sut.RegisterContractAsync(
            command, 
            customerServiceMock.Object, 
            productServiceMock.Object);

        // Assert
        sut.IsValid.Should().BeFalse();
        sut.GetBusinessRuleViolations().Should().ContainSingle(
            "Invalid contract term. The term should be at least 5 years.");
    }   

    [TestMethod]
    public async Task RegisterContract_WithTooLongContractTerm_ShouldYieldViolation()
    {
        // Arrange
        string aggregateId = "CTR-20220424-0001";
        var command = RegisterContractV2Builder.Build(aggregateId)
            with { EndDate = new DateTime(2085, 4, 24, 18, 33, 5) };
        var customerServiceMock = CustomerServiceMock.ForExistingCustomer(command.CustomerNumber);
        var productServiceMock = ProductServiceMock.ForExistingProduct(command.ProductNumber);
        var sut = new Contract(aggregateId);

        // Act
        await sut.RegisterContractAsync(
            command, 
            customerServiceMock.Object, 
            productServiceMock.Object);

        // Assert
        sut.IsValid.Should().BeFalse();
        sut.GetBusinessRuleViolations().Should().ContainSingle(
            "Invalid contract term. The term should be no longer than 50 years.");
    }          

    [TestMethod]
    public async Task RegisterContract_WithTooLargeAmount_ShouldYieldViolation()
    {
        // Arrange
        string aggregateId = "CTR-20220424-0001";
        var command = RegisterContractV2Builder.Build(aggregateId)
            with { Amount = 20000000 };
        var customerServiceMock = CustomerServiceMock.ForExistingCustomer(command.CustomerNumber);
        var productServiceMock = ProductServiceMock.ForExistingProduct(command.ProductNumber);
        var sut = new Contract(aggregateId);        

        // Act
        await sut.RegisterContractAsync(
            command, 
            customerServiceMock.Object, 
            productServiceMock.Object);

        // Assert
        sut.IsValid.Should().BeFalse();
        sut.GetBusinessRuleViolations().Should().ContainSingle(
            "Invalid PaymentPeriod. PaymentPeriod 'Yearly' is only allowed for contracts below 5.000.000 euros.");
    }   

    [TestMethod]
    public async Task RegisterContract_WithAmountMoreThan5mln_AndPaymentPeriodYearly_ShouldYieldViolation()
    {
        // Arrange
        string aggregateId = "CTR-20220424-0001";
        var command = RegisterContractV2Builder.Build(aggregateId)
            with { Amount = 6000000, PaymentPeriod = PaymentPeriod.Yearly };
        var customerServiceMock = CustomerServiceMock.ForExistingCustomer(command.CustomerNumber);
        var productServiceMock = ProductServiceMock.ForExistingProduct(command.ProductNumber);
        var sut = new Contract(aggregateId);          

        // Act
        await sut.RegisterContractAsync(
            command, 
            customerServiceMock.Object, 
            productServiceMock.Object);

        // Assert
        sut.IsValid.Should().BeFalse();
        sut.GetBusinessRuleViolations().Should().ContainSingle(
            "Invalid PaymentPeriod. PaymentPeriod 'Yearly' is only allowed for contracts below 5.000.000 euros.");
    }        
}