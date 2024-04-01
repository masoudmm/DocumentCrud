using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentCrud.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDomainEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IndependentCreditNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ExternalCreditNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndependentCreditNotes", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.CheckConstraint("CK_IndependentCreditNote_ExternalCreditNumber_Alphanumeric", "ExternalCreditNumber NOT LIKE '%[^A-Za-z0-9]%'");
                    table.CheckConstraint("CK_IndependentCreditNote_Number_OnlyDigits", "Number NOT LIKE '%[^0-9]%'");
                    table.CheckConstraint("CK_IndependentCreditNote_TotalAmount_Greater_Then_Zero", "TotalAmount < 0");
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalInvoiceNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.CheckConstraint("CK_Invoice_ExternalInvoiceNumber_Alphanumeric", "ExternalInvoiceNumber NOT LIKE '%[^A-Za-z0-9]%'");
                    table.CheckConstraint("CK_Invoice_Number_OnlyDigits", "Number NOT LIKE '%[^0-9]%'");
                    table.CheckConstraint("CK_Invoice_TotalAmount_Greater_Then_Zero", "TotalAmount > 0");
                });

            migrationBuilder.CreateTable(
                name: "DependentCreditNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentInvoiceId = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ExternalCreditNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DependentCreditNotes", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.CheckConstraint("CK_DependentCreditNote_ExternalCreditNumber_Alphanumeric", "ExternalCreditNumber NOT LIKE '%[^A-Za-z0-9]%'");
                    table.CheckConstraint("CK_DependentCreditNote_Number_OnlyDigits", "Number NOT LIKE '%[^0-9]%'");
                    table.CheckConstraint("CK_DependentCreditNote_TotalAmount_Greater_Then_Zero", "TotalAmount < 0");
                    table.ForeignKey(
                        name: "FK_DependentCreditNotes_Invoices_ParentInvoiceId",
                        column: x => x.ParentInvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DependentCreditNotes_ParentInvoiceId",
                table: "DependentCreditNotes",
                column: "ParentInvoiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DependentCreditNotes");

            migrationBuilder.DropTable(
                name: "IndependentCreditNotes");

            migrationBuilder.DropTable(
                name: "Invoices");
        }
    }
}
