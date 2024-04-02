using DocumentCrud.Domain.Contracts.Persistence;
using DocumentCrud.Domain.Contracts.Persistence.Repositories;

namespace DocumentCrud.Infrastructure.Persistance.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IInvoiceRepository Invoices { get; }
    public IIndependentCreditRepository IndependentCreditNotes { get; }

    public UnitOfWork(ApplicationDbContext context,
        IInvoiceRepository invoiceRepository,
        IIndependentCreditRepository independentCreditNotes)
    {
        _context = context;

        Invoices = invoiceRepository;
        IndependentCreditNotes = independentCreditNotes;
    }

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}