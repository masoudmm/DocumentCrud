using DocumentCrud.Domain.Entities;

namespace DocumentCrud.Domain.Contracts.Persistence;

public interface IUnitOfWork : IDisposable
{
    IInvoiceRepository Invoices { get; }
    IIndependentCreditRepository IndependentCreditNotes { get; }

    Task<int> CommitAsync();
}
