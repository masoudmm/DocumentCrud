using DocumentCrud.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocumentCrud.Infrastructure.Persistance.Configuration;

public class DependentCreditNoteEntityTypeConfiguration : IEntityTypeConfiguration<DependentCreditNote>
{
    public void Configure(EntityTypeBuilder<DependentCreditNote> builder)
    {
        builder.ToTable("Credits");

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
                $"{nameof(DependentCreditNote.Number)} ~ '^[0-9]+$'");

            c.HasCheckConstraint($"CK_DependentCreditNote_{nameof(DependentCreditNote.ExternalNumber)}_OnlyDigits",
                $"{nameof(DependentCreditNote.Number)} ~ '^[0-9]+$'");

            c.HasCheckConstraint($"CK_DependentCreditNote_{nameof(DependentCreditNote.ExternalNumber)}_Alphanumeric",
                $"CHECK ({nameof(DependentCreditNote.ExternalNumber)} ~ '^[A-Za-z0-9]+$')");

            c.HasCheckConstraint($"CK_DependentCreditNote_{nameof(DependentCreditNote.TotalAmount)}_Greater_Then_Zero",
                $"CHECK ({nameof(DependentCreditNote.TotalAmount)} < 0");
        });


        builder.Property(c => c.ExternalNumber)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(c => c.TotalAmount)
            .IsRequired();
    }
}
