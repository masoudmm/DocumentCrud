using DocumentCrud.Application.Common;
using DocumentCrud.Domain.CreditAggregate;
using DocumentCrud.Domain.InvoiceAggregate;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DocumentCrud.Infrastructure.Persistance;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Invoice> Invoices => Set<Invoice>();

    public DbSet<DependentCreditNote> DependentCreditNotes => Set<DependentCreditNote>();

    public DbSet<IndependentCreditNote> IndependentCreditNotes => Set<IndependentCreditNote>();
}
