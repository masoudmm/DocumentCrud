using DocumentCrud.Domain.CreditAggregate;
using DocumentCrud.Domain.InvoiceAggregate;
using Microsoft.EntityFrameworkCore;

namespace DocumentCrud.Application.Common;

public interface IApplicationDbContext
{
    DbSet<Invoice> Invoices { get; }
    DbSet<DependentCreditNote> DependentCreditNotes { get; }
    DbSet<IndependentCreditNote> IndependentCreditNotes { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

