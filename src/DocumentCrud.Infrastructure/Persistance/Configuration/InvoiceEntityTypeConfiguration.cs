using DocumentCrud.Domain.Entities;
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
                $"{nameof(Invoice.Number)} ~ '^[0-9]+$'");

            c.HasCheckConstraint($"CK_Invoice_{nameof(Invoice.ExternalNumber)}_Alphanumeric",
                $"CHECK ({nameof(Invoice.ExternalNumber)} ~ '^[A-Za-z0-9]+$')");

            c.HasCheckConstraint($"CK_Invoice_{nameof(Invoice.TotalAmount)}_Greater_Then_Zero",
                $"CHECK ({nameof(Invoice.TotalAmount)} > 0");
        });


        builder.Property(c => c.ExternalNumber)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(c => c.TotalAmount)
            .IsRequired();

        builder.HasMany(i => i.DependentCreditNotes)
            .WithOne(dct => dct.ParentInvoice)
            .HasForeignKey(dct => dct.ParentInvoiceId);
    }
}