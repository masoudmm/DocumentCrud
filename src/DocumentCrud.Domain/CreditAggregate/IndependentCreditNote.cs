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
        AccountingDocumentStatus status,
        decimal totalAmount)
    {
        if (status == AccountingDocumentStatus.Approved)
        {
            throw new DomainException("Independent credit note Cannot be created with Approved status");
        }

        return new IndependentCreditNote(number,
        externalNumber,
        status,
        totalAmount);
    }

    public void Delete()
    {
    }
}
