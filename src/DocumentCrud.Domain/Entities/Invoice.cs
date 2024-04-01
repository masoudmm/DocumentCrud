namespace DocumentCrud.Domain.Entities;

public class Invoice : AccountingDocument
{
    private readonly HashSet<DependentCreditNote> _dependentCreditNotes;

    public IReadOnlyList<DependentCreditNote> DependentCreditNotes => _dependentCreditNotes.ToList();

    private Invoice(string number,
        string externalNumber,
        AccountingDocumentStatus status,
        decimal totalAmount) 
    {
        Number = number;
        ExternalNumber = externalNumber;
        Status = status;
        TotalAmount = totalAmount;
    }

    public void Edit(string number,
        string externalNumber,
        AccountingDocumentStatus status, 
        decimal totalAmount)
    {
        //We can check some business rules and create events here

        Number = number;
        ExternalNumber = externalNumber;
        Status = status;
        TotalAmount = totalAmount;
    }

    public void AddDependentCredit(string number,
        string externalNumber,
        AccountingDocumentStatus status,
        decimal totalAmount)
    {
        var newDependentCredit = DependentCreditNote.CreateNew(number,
            externalNumber, 
            status, 
            totalAmount,
            this);

        _dependentCreditNotes.Add(newDependentCredit);
    }

    public void EditDependentCredit(int id,
        string number,
        string externalNumber,
        AccountingDocumentStatus status,
        decimal totalAmount)
    {
        var dependentCreditToEdit = DependentCreditNotes.First(x => x.Id == id);


        //TODO: edit the dependent credit
    }

    public static Invoice CreateNewInvoice(string number,
        string externalNumber,
        AccountingDocumentStatus status,
        decimal totalAmount)
    {
        //We can check some business rules and create events here

        return new Invoice(number,
        externalNumber,
        status,
        totalAmount);
    }
}
