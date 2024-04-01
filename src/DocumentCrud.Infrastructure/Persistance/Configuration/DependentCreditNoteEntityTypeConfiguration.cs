using DocumentCrud.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocumentCrud.Infrastructure.Persistance.Configuration;

public class DependentCreditNoteEntityTypeConfiguration : IEntityTypeConfiguration<DependentCreditNote>
{
    public void Configure(EntityTypeBuilder<DependentCreditNote> builder)
    {
        builder.ToTable("DependentCreditNotes");

        builder.HasKey(c => c.Id)
            .IsClustered();

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Number)
            .HasMaxLength(10)
            .IsRequired();

        builder.ToTable(c =>
        {
            c.HasCheckConstraint($"CK_DependentCreditNote_{nameof(DependentCreditNote.Number)}_OnlyDigits",
                $"{nameof(DependentCreditNote.Number)} NOT LIKE '%[^0-9]%'");

            c.HasCheckConstraint($"CK_DependentCreditNote_{nameof(DependentCreditNote.ExternalCreditNumber)}_Alphanumeric",
                $"{nameof(DependentCreditNote.ExternalCreditNumber)} NOT LIKE '%[^A-Za-z0-9]%'");

            c.HasCheckConstraint($"CK_DependentCreditNote_{nameof(DependentCreditNote.TotalAmount)}_Greater_Then_Zero",
                $"{nameof(DependentCreditNote.TotalAmount)} < 0");
        });


        builder.Property(c => c.ExternalCreditNumber)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(c => c.TotalAmount)
            .IsRequired();
    }
}
