using DocumentCrud.Domain.Contracts.Persistence.Repositories;
using DocumentCrud.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace DocumentCrud.Infrastructure.Persistance.Repositories;

internal class InvoiceRepository : IInvoiceRepository
{
    private readonly ApplicationDbContext _context;

    public InvoiceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Invoice entity)
    {
        await _context.Invoices
            .AddAsync(entity);
    }

    public async Task<List<Invoice>> GetAllAsync()
    {
        return await _context.Invoices
            .ToListAsync();
    }

    public async Task<Invoice> GetByIdAsync(int id)
    {
        return await _context.Invoices
            .FirstAsync( i => i.Id == id);
    }

    public int Remove(Invoice entity)
    {
        var invoice = _context.Invoices
            .Remove(entity);

        return invoice.Entity
            .Id;
    }

    public async Task Update(Invoice invoice)
    {
        var invoiceToBeUpdated = await _context.Invoices
            .Include(i => i.DependentCreditNotes)
            .FirstAsync(i => i.Id == invoice.Id);

        invoiceToBeUpdated.Edit(invoice.Number,
            invoice.ExternalInvoiceNumber,
            invoice.Status,
            invoice.TotalAmount,
            invoice.DependentCreditNotes);
    }
}