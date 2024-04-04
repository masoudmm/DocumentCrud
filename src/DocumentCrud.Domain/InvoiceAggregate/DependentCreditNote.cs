using DocumentCrud.Domain.BaseEntities;
using DocumentCrud.Domain.Exception;

namespace DocumentCrud.Domain.InvoiceAggregate;

public class DependentCreditNote : CreditDocument
{
    public Invoice ParentInvoice { get; private set; }

    public string ParentInvoiceNumber => ParentInvoice.Number;

    private DependentCreditNote()
    {
    }

    public DependentCreditNote(
        string number,
        string externalCreditNumber,
        decimal totalAmount) : base()
    {
        Number = number;
        ExternalCreditNumber = externalCreditNumber;
        TotalAmount = totalAmount;
    }

    public void Edit(string number,
        string externalCreditNumber,
        AccountingDocumentStatus status,
        decimal totalAmount)
    {
        if (Status == AccountingDocumentStatus.Approved)
        {
            throw new DomainException("Approved dependent credit cannot be edited");
        }

        Number = number;
        ExternalCreditNumber = externalCreditNumber;
        Status = status;
        TotalAmount = totalAmount;
    }

    public void Delete()
    {
        if (Status == AccountingDocumentStatus.Approved)
        {
            throw new DomainException("Approved dependent credit note Cannot be deleted");
        }
    }
}