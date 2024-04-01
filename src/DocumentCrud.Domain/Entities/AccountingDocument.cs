namespace DocumentCrud.Domain.Entities;

public abstract class AccountingDocument
{
    public int Id { get; private set; }

    public string Number { get; protected set; } 

    public AccountingDocumentStatus Status { get; protected set; }
    
    public decimal TotalAmount { get; protected set; }
}