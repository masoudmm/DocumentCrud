﻿// <auto-generated />
using DocumentCrud.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DocumentCrud.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DocumentCrud.Domain.Entities.DependentCreditNote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ExternalCreditNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("ParentInvoiceId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"));

                    b.HasIndex("ParentInvoiceId");

                    b.ToTable("DependentCreditNotes", null, t =>
                        {
                            t.HasCheckConstraint("CK_DependentCreditNote_ExternalCreditNumber_Alphanumeric", "ExternalCreditNumber NOT LIKE '%[^A-Za-z0-9]%'");

                            t.HasCheckConstraint("CK_DependentCreditNote_Number_OnlyDigits", "Number NOT LIKE '%[^0-9]%'");

                            t.HasCheckConstraint("CK_DependentCreditNote_TotalAmount_Greater_Then_Zero", "TotalAmount < 0");
                        });
                });

            modelBuilder.Entity("DocumentCrud.Domain.Entities.IndependentCreditNote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ExternalCreditNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"));

                    b.ToTable("IndependentCreditNotes", null, t =>
                        {
                            t.HasCheckConstraint("CK_IndependentCreditNote_ExternalCreditNumber_Alphanumeric", "ExternalCreditNumber NOT LIKE '%[^A-Za-z0-9]%'");

                            t.HasCheckConstraint("CK_IndependentCreditNote_Number_OnlyDigits", "Number NOT LIKE '%[^0-9]%'");

                            t.HasCheckConstraint("CK_IndependentCreditNote_TotalAmount_Greater_Then_Zero", "TotalAmount < 0");
                        });
                });

            modelBuilder.Entity("DocumentCrud.Domain.Entities.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ExternalInvoiceNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    SqlServerKeyBuilderExtensions.IsClustered(b.HasKey("Id"));

                    b.ToTable("Invoices", null, t =>
                        {
                            t.HasCheckConstraint("CK_Invoice_ExternalInvoiceNumber_Alphanumeric", "ExternalInvoiceNumber NOT LIKE '%[^A-Za-z0-9]%'");

                            t.HasCheckConstraint("CK_Invoice_Number_OnlyDigits", "Number NOT LIKE '%[^0-9]%'");

                            t.HasCheckConstraint("CK_Invoice_TotalAmount_Greater_Then_Zero", "TotalAmount > 0");
                        });
                });

            modelBuilder.Entity("DocumentCrud.Domain.Entities.DependentCreditNote", b =>
                {
                    b.HasOne("DocumentCrud.Domain.Entities.Invoice", "ParentInvoice")
                        .WithMany("DependentCreditNotes")
                        .HasForeignKey("ParentInvoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ParentInvoice");
                });

            modelBuilder.Entity("DocumentCrud.Domain.Entities.Invoice", b =>
                {
                    b.Navigation("DependentCreditNotes");
                });
#pragma warning restore 612, 618
        }
    }
}
