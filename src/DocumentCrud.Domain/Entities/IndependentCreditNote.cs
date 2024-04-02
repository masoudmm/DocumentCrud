namespace DocumentCrud.Domain.Entities;

public class IndependentCreditNote : CreditDocument, IAggregateRoot
{
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
        //We can check some business rules and create events here

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
        //We can check some business rules and create events here

        return new IndependentCreditNote(number,
        externalNumber,
        status,
        totalAmount);
    }

    public void Delete()
    {
    }
}
