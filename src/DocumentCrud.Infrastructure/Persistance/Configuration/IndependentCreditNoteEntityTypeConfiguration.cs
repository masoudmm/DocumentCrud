using DocumentCrud.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocumentCrud.Infrastructure.Persistance.Configuration;

public class IndependentCreditNoteEntityTypeConfiguration : IEntityTypeConfiguration<IndependentCreditNote>
{
    public void Configure(EntityTypeBuilder<IndependentCreditNote> builder)
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
            c.HasCheckConstraint($"CK_IndependentCreditNote_{nameof(IndependentCreditNote.Number)}_OnlyDigits",
                $"{nameof(IndependentCreditNote.Number)} ~ '^[0-9]+$'");

            c.HasCheckConstraint($"CK_IndependentCreditNote_{nameof(IndependentCreditNote.ExternalNumber)}_Alphanumeric",
                $"CHECK ({nameof(IndependentCreditNote.ExternalNumber)} ~ '^[A-Za-z0-9]+$')");

            c.HasCheckConstraint($"CK_IndependentCreditNote_{nameof(IndependentCreditNote.TotalAmount)}_Greater_Then_Zero",
                $"CHECK ({nameof(IndependentCreditNote.TotalAmount)} < 0");
        });


        builder.Property(c => c.ExternalNumber)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(c => c.TotalAmount)
            .IsRequired();
    }
}
