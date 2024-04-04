using DocumentCrud.Domain.BaseEntities;
using DocumentCrud.Domain.Entities;
using DocumentCrud.Domain.Exception;

namespace DocumentCrud.Domain.CreditAggregate;

public class IndependentCreditNote : CreditDocument, IEntity, IAggregateRoot
{
    private IndependentCreditNote() { }

    public IndependentCreditNote(string number,
        string externalCreditNumber,
        decimal totalAmount)
    {
        if (number.Equals(externalCreditNumber,
            StringComparison.OrdinalIgnoreCase))
        {
            throw new DomainException("invoice number cannot be the same as externalInvoiceNumber");
        }

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
            throw new DomainException("Approved Independent credit note Cannot be edited");
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
            throw new DomainException("Approved Independent credit note Cannot be deleted");
        }
    }
}
