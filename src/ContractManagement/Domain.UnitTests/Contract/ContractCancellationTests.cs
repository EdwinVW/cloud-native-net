namespace Domain.UnitTests.ContractAggregate;

[TestClass]
public class ContractCancellationTests
{
    [TestMethod]
    public void CancelContract_ShouldCancelContract()
    {
        // Arrange
        string aggregateId = "CTR-20220424-0001";
        var contractRegistered = ContractRegisteredV2Builder.Build(aggregateId);
        var cancelContract = CancelContractBuilder.Build(aggregateId);
        var sut = new Contract(new List<Event> { contractRegistered });

        // Act
        sut.CancelContract(cancelContract);

        // Assert
        sut.IsValid.Should().BeTrue();
        sut.ContractNumber!.Value.Should().BeEquivalentTo(contractRegistered.ContractNumber);
        sut.CustomerNumber!.Value.Should().BeEquivalentTo(contractRegistered.CustomerNumber);
        sut.ProductNumber!.Value.Should().BeEquivalentTo(contractRegistered.ProductNumber);
        sut.Amount!.Value.Should().Be(contractRegistered.Amount);
        sut.ContractTerm!.StartDate.Should().BeSameDateAs(contractRegistered.StartDate);
        sut.ContractTerm.EndDate.Should().BeSameDateAs(contractRegistered.EndDate);
        sut.PaymentPeriod.Should().Be(contractRegistered.PaymentPeriod);
        sut.Cancelled.Should().BeTrue();

        sut.GetDomainEvents().Should().ContainSingle(e => e is ContractCancelled)
            .Which.Should().BeEquivalentTo(cancelContract, options => options
                .ExcludingMissingMembers()
                .Excluding(e => e.Type));          
    }
}