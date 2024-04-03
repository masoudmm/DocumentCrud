using DocumentCrud.Domain.BaseEntities;
using DocumentCrud.Domain.CreditAggregate;
using DocumentCrud.Domain.Exception;

namespace Domain.Tests.Aggregates;

public class IndependentCreditTests
{
    [Fact]
    public void Create_New_IndependentCredit_Should_Be_Successfull()
    {
        // Arrange
        string number = "1234567890";
        string externalNumber = "1234567icr1";
        AccountingDocumentStatus status = AccountingDocumentStatus.WaitingForApproval;
        decimal totalAmount = 1000m;

        // Act
        var independentCredit = IndependentCreditNote.CreateNew(number,
            externalNumber,
            totalAmount);

        // Assert
        Assert.NotNull(independentCredit);
        Assert.Equal(number, independentCredit.Number);
        Assert.Equal(externalNumber, independentCredit.ExternalCreditNumber);
        Assert.Equal(status, independentCredit.Status);
        Assert.Equal(totalAmount, independentCredit.TotalAmount);
    }


    [Fact]
    public void Edit_IndependentCredit_Should_Be_Successfull()
    {
        // Arrange
        string number = "1234567890";
        string externalNumber = "1234567icr1";
        AccountingDocumentStatus status = AccountingDocumentStatus.WaitingForApproval;
        decimal totalAmount = 1000m;

        var independentCredit = IndependentCreditNote.CreateNew(number,
            externalNumber,
            totalAmount);

        decimal newTotalAmount = 1001m;

        // Act
        independentCredit.Edit(number,
            externalNumber,
            status,
            newTotalAmount);

        // Assert
        Assert.NotNull(independentCredit);
        Assert.Equal(number, independentCredit.Number);
        Assert.Equal(externalNumber, independentCredit.ExternalCreditNumber);
        Assert.Equal(status, independentCredit.Status);
        Assert.Equal(newTotalAmount, independentCredit.TotalAmount);
    }

    [Fact]
    public void Edit_Approved_IndependentCredit_Should_Fail()
    {
        // Arrange
        string number = "1234567890";
        string externalNumber = "1234567icr1";
        AccountingDocumentStatus status = AccountingDocumentStatus.WaitingForApproval;
        decimal totalAmount = 1000m;

        var independentCredit = IndependentCreditNote.CreateNew(number,
            externalNumber,
            totalAmount);

        AccountingDocumentStatus newStatus = AccountingDocumentStatus.Approved;

        independentCredit.Edit(number,
            externalNumber,
            newStatus,
            totalAmount);

        decimal newTotalAmount = 1001m;

        // Act
        // Assert
        Assert.Throws<DomainException>(() => independentCredit.Edit(number,
            externalNumber,
            newStatus,
            newTotalAmount));
    }
}