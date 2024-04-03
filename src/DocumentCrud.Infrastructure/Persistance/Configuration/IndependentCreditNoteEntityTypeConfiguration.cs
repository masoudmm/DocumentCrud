using DocumentCrud.Domain.CreditAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocumentCrud.Infrastructure.Persistance.Configuration;

public class IndependentCreditNoteEntityTypeConfiguration : IEntityTypeConfiguration<IndependentCreditNote>
{
    public void Configure(EntityTypeBuilder<IndependentCreditNote> builder)
    {
        builder.ToTable("IndependentCreditNotes");

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
                $"{nameof(IndependentCreditNote.Number)} NOT LIKE '%[^0-9]%'");

            c.HasCheckConstraint($"CK_IndependentCreditNote_{nameof(IndependentCreditNote.ExternalCreditNumber)}_Alphanumeric",
                $"{nameof(IndependentCreditNote.ExternalCreditNumber)} NOT LIKE '%[^A-Za-z0-9]%'");

            c.HasCheckConstraint($"CK_IndependentCreditNote_{nameof(IndependentCreditNote.TotalAmount)}_Greater_Then_Zero",
                $"{nameof(IndependentCreditNote.TotalAmount)} < 0");
        });


        builder.Property(c => c.ExternalCreditNumber)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(c => c.TotalAmount)
            .IsRequired();
    }
}
