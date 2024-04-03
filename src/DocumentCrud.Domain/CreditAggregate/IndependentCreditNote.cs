using DocumentCrud.Domain.BaseEntities;
using DocumentCrud.Domain.Entities;
using DocumentCrud.Domain.Exception;

namespace DocumentCrud.Domain.CreditAggregate;

public class IndependentCreditNote : CreditDocument, IEntity, IAggregateRoot
{
    private IndependentCreditNote() { }

    private IndependentCreditNote(string number,
        string externalCreditNumber,
        AccountingDocumentStatus status,
        decimal totalAmount)
    {
        Number = number;
        ExternalCreditNumber = externalCreditNumber;
        Status = status;
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

    public static IndependentCreditNote CreateNew(string number,
        string externalNumber,
        decimal totalAmount)
    {
        if (number.Equals(externalNumber,
            StringComparison.OrdinalIgnoreCase))
        {
            throw new DomainException("invoice number cannot be the same as externalInvoiceNumber");
        }

        return new IndependentCreditNote(number,
            externalNumber,
            AccountingDocumentStatus.WaitingForApproval,
            totalAmount);
    }

    public void Delete()
    {
    }
}
