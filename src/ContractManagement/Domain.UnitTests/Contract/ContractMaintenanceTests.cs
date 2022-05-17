namespace Domain.UnitTests.ContractAggregate;

[TestClass]
public class ContractMaintenanceTests
{
    [TestMethod]
    public void ChangeContractAmount_ShouldUpdateContract()
    {
        // Arrange
        var contractRegistered = ContractRegisteredV2Builder.Build();
        var changeContractAmount = ChangeContractAmountBuilder.Build();
        var sut = new Contract(
            contractRegistered.ContractNumber, 
            new List<Event> { contractRegistered });

        // Act
        sut.ChangeContractAmount(changeContractAmount);

        // Assert
        sut.IsConsistent.Should().BeTrue();
        sut.ContractNumber.Value.Should().BeEquivalentTo(contractRegistered.ContractNumber);
        sut.CustomerNumber!.Value.Should().BeEquivalentTo(contractRegistered.CustomerNumber);
        sut.ProductNumber!.Value.Should().BeEquivalentTo(contractRegistered.ProductNumber);
        sut.Amount!.Value.Should().Be(changeContractAmount.NewAmount);
        sut.ContractTerm!.StartDate.Should().BeSameDateAs(contractRegistered.StartDate);
        sut.ContractTerm.EndDate.Should().BeSameDateAs(contractRegistered.EndDate);
        sut.PaymentPeriod.Should().Be(contractRegistered.PaymentPeriod);
        sut.Cancelled.Should().BeFalse();

        sut.GetDomainEvents().Should().ContainSingle(e => e is ContractAmountChanged)
            .Which.Should().BeEquivalentTo(changeContractAmount, options => options
                .ExcludingMissingMembers()
                .Excluding(e => e.Type));
    }

    [TestMethod]
    public void ChangeContractAmount_WithTooSmallAmount_ShouldYieldViolation()
    {
        // Arrange
        var contractRegistered = ContractRegisteredV2Builder.Build();
        var changeContractAmount = ChangeContractAmountBuilder.Build()
            with { NewAmount = 500 }; // minimum amount is 1000
        var sut = new Contract(
            contractRegistered.ContractNumber, 
            new List<Event> { contractRegistered });

        // Act
        sut.ChangeContractAmount(changeContractAmount);

        // Assert
        sut.IsConsistent.Should().BeFalse();
        sut.GetBusinessRuleViolations().Should().ContainSingle(
            "Invalid amount. The amount on a contract must be between 1.000 and 10.000.000 Euros.");
    }

    [TestMethod]
    public void ChangeContractAmount_WithTooLargeAmount_ShouldYieldViolation()
    {
        // Arrange
        var contractRegistered = ContractRegisteredV2Builder.Build();
        var changeContractAmount = ChangeContractAmountBuilder.Build()
            with { NewAmount = 20000000 }; // maximum amount is 10000000
        var sut = new Contract(
            contractRegistered.ContractNumber, 
            new List<Event> { contractRegistered });

        // Act
        sut.ChangeContractAmount(changeContractAmount);

        // Assert
        sut.IsConsistent.Should().BeFalse();
        sut.GetBusinessRuleViolations().Should().ContainSingle(
            "Invalid amount. The amount on a contract must be between 1.000 and 10.000.000 Euros.");
    }

    [TestMethod]
    public void ChangeContractTerm_ShouldUpdateContract()
    {
        // Arrange
        var contractRegistered = ContractRegisteredV2Builder.Build();
        var changeContractTerm = ChangeContractTermBuilder.Build();
        var sut = new Contract(
            contractRegistered.ContractNumber, 
            new List<Event> { contractRegistered });

        // Act
        sut.ChangeContractTerm(changeContractTerm);

        // Assert
        sut.IsConsistent.Should().BeTrue();
        sut.ContractNumber.Value.Should().BeEquivalentTo(contractRegistered.ContractNumber);
        sut.CustomerNumber!.Value.Should().BeEquivalentTo(contractRegistered.CustomerNumber);
        sut.ProductNumber!.Value.Should().BeEquivalentTo(contractRegistered.ProductNumber);
        sut.Amount!.Value.Should().Be(contractRegistered.Amount);
        sut.ContractTerm!.StartDate.Should().BeSameDateAs(changeContractTerm.StartDate);
        sut.ContractTerm.EndDate.Should().BeSameDateAs(changeContractTerm.EndDate);
        sut.PaymentPeriod.Should().Be(contractRegistered.PaymentPeriod);
        sut.Cancelled.Should().BeFalse();

        sut.GetDomainEvents().Should().ContainSingle(e => e is ContractTermChanged)
            .Which.Should().BeEquivalentTo(changeContractTerm, options => options
                .ExcludingMissingMembers()
                .Excluding(e => e.Type));
    }

    [TestMethod]
    public void ChangeContractTerm_WithTooShortContractTerm_ShouldYieldViolation()
    {
        var contractRegistered = ContractRegisteredV2Builder.Build();
        var changeContractTerm = ChangeContractTermBuilder.Build()
            with { EndDate = new DateTime(2025, 4, 24, 18, 33, 5) };
        var sut = new Contract(
            contractRegistered.ContractNumber, 
            new List<Event> { contractRegistered });

        // Act
        sut.ChangeContractTerm(changeContractTerm);

        // Assert
        sut.IsConsistent.Should().BeFalse();
        sut.GetBusinessRuleViolations().Should().ContainSingle(
            "Invalid contract term. The term should be at least 5 years.");
    }

    [TestMethod]
    public void ChangeContractTerm_WithTooLongContractTerm_ShouldYieldViolation()
    {
        var contractRegistered = ContractRegisteredV2Builder.Build();
        var changeContractTerm = ChangeContractTermBuilder.Build()
            with { EndDate = new DateTime(2085, 4, 24, 18, 33, 5) };
        var sut = new Contract(
            contractRegistered.ContractNumber, 
            new List<Event> { contractRegistered });

        // Act
        sut.ChangeContractTerm(changeContractTerm);

        // Assert
        sut.IsConsistent.Should().BeFalse();
        sut.GetBusinessRuleViolations().Should().ContainSingle(
            "Invalid contract term. The term should be no longer than 50 years.");
    }

    [TestMethod]
    public void ChangeContractAmountToMoreThan5mln_ForContractWithPaymentPeriodYearly_ShouldYieldViolation()
    {
        // Arrange
        var contractRegistered = ContractRegisteredV2Builder.Build()
            with { PaymentPeriod = PaymentPeriod.Yearly };
        var changeContractAmount = ChangeContractAmountBuilder.Build()
            with { NewAmount = 6000000 };
        var sut = new Contract(
            contractRegistered.ContractNumber, 
            new List<Event> { contractRegistered });

        // Act
        sut.ChangeContractAmount(changeContractAmount);

        // Assert
        sut.IsConsistent.Should().BeFalse();
        sut.GetBusinessRuleViolations().Should().ContainSingle(
            "Invalid amount. The amount on a contract must be between 1.000 and 10.000.000 Euros.");
    }
}