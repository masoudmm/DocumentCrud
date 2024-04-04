using DocumentCrud.Domain.BaseEntities;
using DocumentCrud.Domain.Exception;
using DocumentCrud.Domain.InvoiceAggregate;

namespace Domain.Tests.Aggregates;
public class InvoiceTests
{
    [Fact]
    public void Create_New_Invoice_Should_Be_Successfull()
    {
        // Arrange
        string number = "1234567890";
        string externalNumber = "1234567inv1";
        AccountingDocumentStatus status = AccountingDocumentStatus.WaitingForApproval;
        decimal totalAmount = 1000m;

        // Act
        var invoice = new Invoice(number,
            externalNumber,
            totalAmount);

        // Assert
        Assert.NotNull(invoice);
        Assert.Equal(number, invoice.Number);
        Assert.Equal(externalNumber, invoice.ExternalInvoiceNumber);
        Assert.Equal(status, invoice.Status);
        Assert.Equal(totalAmount, invoice.TotalAmount);
        Assert.Empty(invoice.DependentCreditNotes);
    }

    [Fact]
    public void Edit_Invoice_Should_Be_Successfull()
    {
        // Arrange
        string number = "1234567890";
        string externalNumber = "1234567inv1";
        AccountingDocumentStatus status = AccountingDocumentStatus.WaitingForApproval;
        decimal totalAmount = 1000m;

        var invoice = new Invoice(number,
            externalNumber,
            totalAmount);

        decimal newTotalAmount = 1001m;

        // Act
        invoice.Edit(number,
            externalNumber,
            status,
            newTotalAmount);

        // Assert
        Assert.NotNull(invoice);
        Assert.Equal(number, invoice.Number);
        Assert.Equal(externalNumber, invoice.ExternalInvoiceNumber);
        Assert.Equal(status, invoice.Status);
        Assert.Equal(newTotalAmount, invoice.TotalAmount);
        Assert.Empty(invoice.DependentCreditNotes);
    }

    [Fact]
    public void Edit_Approved_Invoice_Should_Fail()
    {
        // Arrange
        string number = "1234567890";
        string externalNumber = "1234567inv1";
        decimal totalAmount = 1000m;

        var invoice = new Invoice(number,
            externalNumber,
            totalAmount);

        AccountingDocumentStatus newStatus = AccountingDocumentStatus.Approved;

        invoice.Edit(number,
            externalNumber,
            newStatus,
            totalAmount);

        decimal newTotalAmount = 1001m;

        // Act
        // Assert
        Assert.Throws<DomainException>(() => invoice.Edit(number,
            externalNumber,
            newStatus,
            newTotalAmount));
    }




    [Fact]
    public void Invoice_Add_Dependent_Credit_Should_Be_Successfull()
    {
        // Arrange
        var invoice = new Invoice("1234567890",
            "1234567iv1",
            1000m);

        string creditNumber = "1234567891";
        string externalCreditNumber = "1234567dc1";
        AccountingDocumentStatus creditStatus = AccountingDocumentStatus.WaitingForApproval;
        decimal creditTotalAmount = 200m;

        var newDependentCredit = new DependentCreditNote(creditNumber,
            externalCreditNumber,
            creditTotalAmount);

        // Act
        invoice.AddDependentCredit(newDependentCredit);

        // Assert
        Assert.Single(invoice.DependentCreditNotes);

        var dependentCreditNote = invoice.DependentCreditNotes.First();
        Assert.Equal(creditNumber, dependentCreditNote.Number);
        Assert.Equal(externalCreditNumber, dependentCreditNote.ExternalCreditNumber);
        Assert.Equal(creditStatus, dependentCreditNote.Status);
        Assert.Equal(creditTotalAmount, dependentCreditNote.TotalAmount);
    }

    [Fact]
    public void Invoice_Edit_Dependent_Credit_Should_Be_Successfull()
    {
        // Arrange
        var invoice = new Invoice("1234567890",
            "1234567iv1",
            1000m);

        string creditNumber = "1234567891";
        string externalCreditNumber = "1234567dc1";
        AccountingDocumentStatus creditStatus = AccountingDocumentStatus.WaitingForApproval;
        decimal creditTotalAmount = 200m;

        var newDependentCredit = new DependentCreditNote(creditNumber,
            externalCreditNumber,
            creditTotalAmount);

        invoice.AddDependentCredit(newDependentCredit);

        decimal newCreditTotalAmount = 201m;

        // Act
        invoice.EditDependentCredit(0,
            creditNumber,
            externalCreditNumber,
            creditStatus,
            newCreditTotalAmount);

        // Assert
        Assert.Single(invoice.DependentCreditNotes);

        var dependentCreditNote = invoice.DependentCreditNotes.First();
        Assert.Equal(creditNumber, dependentCreditNote.Number);
        Assert.Equal(externalCreditNumber, dependentCreditNote.ExternalCreditNumber);
        Assert.Equal(creditStatus, dependentCreditNote.Status);
        Assert.Equal(newCreditTotalAmount, dependentCreditNote.TotalAmount);
    }

    [Fact]
    public void Approved_Invoice_Add_Dependent_Credit_Should_Fail()
    {
        // Arrange
        var invoice = new Invoice("1234567890",
            "1234567iv1",
            1000m);

        invoice.Edit(invoice.Number,
            invoice.ExternalInvoiceNumber,
            AccountingDocumentStatus.Approved,
            invoice.TotalAmount);

        string creditNumber = "1234567891";
        string externalCreditNumber = "1234567dc1";
        decimal creditTotalAmount = 200m;

        var newDependentCredit = new DependentCreditNote(creditNumber,
            externalCreditNumber,
            creditTotalAmount);

        // Act
        // Assert
        Assert.Throws<DomainException>(() => invoice.AddDependentCredit(newDependentCredit));
    }


    [Fact]
    public void Approved_Invoice_Edit_Dependent_Credit_Should_Fail()
    {
        // Arrange
        string creditNumber = "1234567891";
        string externalCreditNumber = "1234567dc1";
        AccountingDocumentStatus creditStatus = AccountingDocumentStatus.WaitingForApproval;
        decimal creditTotalAmount = 200m;

        var invoice = new Invoice("1234567890",
            "1234567iv1",
            1000m);

        invoice.AddDependentCredit(new DependentCreditNote(creditNumber,
            externalCreditNumber,
            creditTotalAmount));

        invoice.Edit(invoice.Number,
            invoice.ExternalInvoiceNumber,
            AccountingDocumentStatus.Approved,
            invoice.TotalAmount);

        decimal newCreditTotalAmount = 201m;

        // Act
        // Assert
        Assert.Throws<DomainException>(() => invoice.EditDependentCredit(0,
            creditNumber,
            externalCreditNumber,
            creditStatus,
            newCreditTotalAmount));
    }

    [Fact]
    public void Approved_Invoice_Edit_Approved_Dependent_Credit_Should_Fail()
    {
        // Arrange
        string creditNumber = "1234567891";
        string externalCreditNumber = "1234567dc1";
        AccountingDocumentStatus creditStatus = AccountingDocumentStatus.WaitingForApproval;
        decimal creditTotalAmount = 200m;

        var invoice = new Invoice("1234567890",
            "1234567iv1",
            1000m);

        var credit = new DependentCreditNote(creditNumber,
            externalCreditNumber,
            creditTotalAmount);
        invoice.AddDependentCredit(credit);

        invoice.EditDependentCredit(credit.Id,
            credit.Number,
            credit.ExternalCreditNumber,
            AccountingDocumentStatus.Approved,
            credit.TotalAmount);
        decimal newCreditTotalAmount = 201m;

        // Act
        // Assert
        Assert.Throws<DomainException>(() => invoice.EditDependentCredit(0,
            creditNumber,
            externalCreditNumber,
            creditStatus,
            newCreditTotalAmount));
    }


    [Fact]
    public void Invoice_Add_Exceeded_Dependent_Credit_Should_Fail()
    {
        // Arrange
        var invoice = new Invoice("1234567890",
            "1234567iv1",
            1000m);

        string dependentNumber = "1234567891";
        string dependentExternalNumber = "1234567dc1";
        decimal dependentTotalAmount = 1001m;

        // Act
        // Assert
        Assert.Throws<DomainException>(() => invoice.AddDependentCredit(new DependentCreditNote(dependentNumber,
            dependentExternalNumber,
            dependentTotalAmount)));
    }
}