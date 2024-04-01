using DocumentCrud.Domain.Contracts.Persistence;
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

    public async Task AddDependentCreditNoteAsync(Invoice invoice,
        string number,
        string externalNumber,
        AccountingDocumentStatus status,
        decimal totalAmount)
    {
        invoice.AddDependentCredit(number,
        externalNumber,
        status,
        totalAmount);
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

    public void Remove(Invoice entity)
    {
        _context.Invoices
            .Remove(entity);
    }

    public async Task Update(Invoice entity)
    {
        var invoice = await _context.Invoices
            .Include(i => i.DependentCreditNotes)
            .FirstAsync(i => i.Id == entity.Id);

        invoice.Edit(invoice.Number,
            invoice.ExternalInvoiceNumber,
            invoice.Status,
            invoice.TotalAmount);
    }
}