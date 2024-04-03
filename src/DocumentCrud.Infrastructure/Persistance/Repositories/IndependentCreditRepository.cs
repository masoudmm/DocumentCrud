using DocumentCrud.Domain.Contracts.Persistence.Repositories;
using DocumentCrud.Domain.CreditAggregate;
using Microsoft.EntityFrameworkCore;

namespace DocumentCrud.Infrastructure.Persistance.Repositories;

internal class IndependentCreditRepository : IIndependentCreditRepository
{
    private readonly ApplicationDbContext _context;

    public IndependentCreditRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(IndependentCreditNote entity)
    {
        await _context.IndependentCreditNotes
            .AddAsync(entity);
    }

    public async Task<List<IndependentCreditNote>> GetAllAsync()
    {
        return await _context.IndependentCreditNotes
            .ToListAsync();
    }

    public async Task<IndependentCreditNote> GetByIdAsync(int id)
    {
        return await _context.IndependentCreditNotes
            .FirstAsync(i => i.Id == id);
    }

    public int Remove(IndependentCreditNote credit)
    {
        var removedCredit = _context.IndependentCreditNotes
            .Remove(credit);

        return removedCredit.Entity
            .Id;
    }

    public async Task Update(IndependentCreditNote entity)
    {
        var independentCreditNote = await _context.IndependentCreditNotes
            .FirstAsync(i => i.Id == entity.Id);

        independentCreditNote.Edit(independentCreditNote.Number, 
            independentCreditNote.ExternalCreditNumber,
            independentCreditNote.Status,
            independentCreditNote.TotalAmount);
    }
}