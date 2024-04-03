using DocumentCrud.Domain.InvoiceAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocumentCrud.Infrastructure.Persistance.Configuration;

public class InvoiceEntityTypeConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");

        builder.HasKey(c => c.Id)
            .IsClustered();

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Number)
            .HasMaxLength(10)
            .IsRequired();

        builder.ToTable(c =>
        {
            c.HasCheckConstraint($"CK_Invoice_{nameof(Invoice.Number)}_OnlyDigits",
                $"{nameof(Invoice.Number)} NOT LIKE '%[^0-9]%'");

            c.HasCheckConstraint($"CK_Invoice_{nameof(Invoice.ExternalInvoiceNumber)}_Alphanumeric",
                $"{nameof(Invoice.ExternalInvoiceNumber)} NOT LIKE '%[^A-Za-z0-9]%'");

            c.HasCheckConstraint($"CK_Invoice_{nameof(Invoice.TotalAmount)}_Greater_Then_Zero",
                $"{nameof(Invoice.TotalAmount)} > 0");
        });


        builder.Property(c => c.ExternalInvoiceNumber)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(c => c.TotalAmount)
            .IsRequired();

        builder.HasMany(i => i.DependentCreditNotes)
            .WithOne(dct => dct.ParentInvoice)
            .HasForeignKey("ParentInvoiceId");
    }
}