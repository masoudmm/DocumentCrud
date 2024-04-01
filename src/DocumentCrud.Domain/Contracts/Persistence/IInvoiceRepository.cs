using DocumentCrud.Domain.Entities;

namespace DocumentCrud.Domain.Contracts.Persistence;

public interface IInvoiceRepository : IRepository<Invoice>
{
    Task AddDependentCreditNoteAsync(Invoice invoice, 
        string number,
        string externalNumber,
        AccountingDocumentStatus status,
        decimal totalAmount);
}