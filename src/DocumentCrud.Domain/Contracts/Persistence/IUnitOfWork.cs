using DocumentCrud.Domain.Contracts.Persistence.Repositories;

namespace DocumentCrud.Domain.Contracts.Persistence;

public interface IUnitOfWork : IDisposable
{
    IInvoiceRepository Invoices { get; }
    IIndependentCreditRepository IndependentCreditNotes { get; }

    Task<int> CommitAsync();
}
