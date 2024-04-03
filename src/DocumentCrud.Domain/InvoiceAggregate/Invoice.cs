using DocumentCrud.Domain.BaseEntities;
using DocumentCrud.Domain.Entities;
using DocumentCrud.Domain.Exception;

namespace DocumentCrud.Domain.InvoiceAggregate;

public class Invoice : AccountingDocument, IEntity, IAggregateRoot
{
    private readonly List<DependentCreditNote> _dependentCreditNotes;

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

    public void Edit(string newNumber,
        string newExternalInvoiceNumber,
        AccountingDocumentStatus newStatus,
        decimal newTotalAmount)
    {
        if (Status == AccountingDocumentStatus.Approved)
        {
            throw new DomainException("Approved invoice Cannot be edited");
        }

        Number = newNumber;
        ExternalInvoiceNumber = newExternalInvoiceNumber;
        Status = newStatus;
        TotalAmount = newTotalAmount;
    }

    public void AddDependentCredit(string number,
        string externaCreditNumber,
        AccountingDocumentStatus creditStatus,
        decimal creditTotalAmount)
    {
        if (Status == AccountingDocumentStatus.Approved)
        {
            throw new DomainException("Approved invoice Cannot be edited");
        }

        var sumOfDependentCreditsAfterEditEdited = _dependentCreditNotes
            .Sum(x => x.TotalAmount) + creditTotalAmount;

        if (Math.Abs(sumOfDependentCreditsAfterEditEdited) > TotalAmount)
        {
            throw new DomainException("Sum of dependent credits of an invoice can't exceed invoice total amount");
        }

        var newDependentCredit = DependentCreditNote.CreateNew(number,
            externaCreditNumber,
            creditStatus,
            creditTotalAmount,
            this);

        _dependentCreditNotes.Add(newDependentCredit);
    }

    public void EditDependentCredit(int creditIdTobeEdited,
        string creditNumber,
        string externalCreditNumber,
        AccountingDocumentStatus creditStatus,
        decimal creditTotalAmount)
    {
        if (Status == AccountingDocumentStatus.Approved)
        {
            throw new DomainException("Approved invoice Cannot be edited");
        }

        if (creditStatus == AccountingDocumentStatus.Approved)
        {
            throw new DomainException("can't edit dependentCredit after approve");
        }

        var sumOfDependentCreditsAfterEditEdited = _dependentCreditNotes
            .Where(x => x.Id != creditIdTobeEdited)
            .Sum(x => x.TotalAmount) + creditTotalAmount;

        if (Math.Abs(sumOfDependentCreditsAfterEditEdited) > TotalAmount)
        {
            throw new DomainException("Sum of dependent credits of an invoice can't exceed invoice total amount");
        }

        var dependentCreditToEdit = _dependentCreditNotes.First(dcn => dcn.Id == creditIdTobeEdited);

        dependentCreditToEdit.Edit(creditNumber,
            externalCreditNumber,
            creditStatus,
            creditTotalAmount);
    }

    public void RemoveDependentCredit(DependentCreditNote dependentCreditNoteToremove)
    {
        if (Status == AccountingDocumentStatus.Approved)
        {
            throw new DomainException("Dependent credits of an approved invoice Cannot be edited");
        }

        var dependentCreditToRemove = _dependentCreditNotes.First(x => x.Id == dependentCreditNoteToremove.Id);
        if (dependentCreditToRemove.Status == AccountingDocumentStatus.Approved)
        {
            throw new DomainException("can't edit dependentCredit after approve");
        }

        _dependentCreditNotes.Remove(dependentCreditToRemove);
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
        if (status == AccountingDocumentStatus.Approved)
        {
            throw new DomainException("Invoice Cannot be created with Approved status");
        }

        return new Invoice(number,
            externalNumber,
            status,
            totalAmount);
    }
}
