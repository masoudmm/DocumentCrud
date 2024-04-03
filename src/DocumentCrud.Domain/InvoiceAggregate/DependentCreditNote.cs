using DocumentCrud.Domain.BaseEntities;

namespace DocumentCrud.Domain.InvoiceAggregate;

public class DependentCreditNote : CreditDocument
{
    public int ParentInvoiceId { get; private set; }
    public Invoice ParentInvoice { get; private set; }

    public string ParentInvoiceNumber => ParentInvoice.Number;

    private DependentCreditNote()
    {
    }

    private DependentCreditNote(string number,
        string externalCreditNumber,
        AccountingDocumentStatus status,
        decimal totalAmount,
        Invoice parentInvoice) : base()
    {
        Number = number;
        ExternalCreditNumber = externalCreditNumber;
        Status = status;
        TotalAmount = totalAmount;
        ParentInvoice = parentInvoice;
    }

    public void Edit(string number,
        string externalCreditNumber,
        AccountingDocumentStatus status,
        decimal totalAmount)
    {
        //We can check some business rules and create events here

        Number = number;
        ExternalCreditNumber = externalCreditNumber;
        Status = status;
        TotalAmount = totalAmount;
    }

    public static DependentCreditNote CreateNew(string number,
        string externalCreditNumber,
        AccountingDocumentStatus status,
        decimal totalAmount,
        Invoice parentInvoice)
    {
        //We can check some business rules and create events here

        return new DependentCreditNote(number,
        externalCreditNumber,
        status,
        totalAmount,
        parentInvoice);
    }
}