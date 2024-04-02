using System.Diagnostics.CodeAnalysis;

namespace DocumentCrud.Domain.Entities;

public class Invoice : AccountingDocument, IAggregateRoot
{
    private readonly HashSet<DependentCreditNote> _dependentCreditNotes;

    public string ExternalInvoiceNumber { get; private set; }

    public IReadOnlyList<DependentCreditNote> DependentCreditNotes => _dependentCreditNotes.ToList();

    private Invoice() { }

    private Invoice(string number,
        string externalInvoiceNumber,
        AccountingDocumentStatus status,
        decimal totalAmount) : base()
    {
        Number = number;
        ExternalInvoiceNumber = externalInvoiceNumber;
        Status = status;
        TotalAmount = totalAmount;
        _dependentCreditNotes = [];
    }

    public void Edit(string number,
        string externalInvoiceNumber,
        AccountingDocumentStatus status, 
        decimal totalAmount,
        IReadOnlyList<DependentCreditNote> dependentCreditNotes)
    {
        Number = number;
        ExternalInvoiceNumber = externalInvoiceNumber;
        Status = status;
        TotalAmount = totalAmount;

        foreach (var dependentCreditNote in dependentCreditNotes)
        {
            _dependentCreditNotes.Add(dependentCreditNote);
        }
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
        var dependentCreditToEdit = _dependentCreditNotes.First(x => x.Id == id);

        dependentCreditToEdit.Edit(number,
            externalNumber,
            status, 
            totalAmount);
    }

    public void Delete()
    {
        _dependentCreditNotes.Clear();
    }

    public static Invoice CreateNew(string number,
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
