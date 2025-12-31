using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceApp.Infrastructure.Persistence.Migrations;

public partial class AddSubhaulerLastInvoiceNoAndInvoiceNumberIndex : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "LastInvoiceNo",
            table: "Subhaulers",
            type: "INTEGER",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Invoices_InvoiceNumber",
            table: "Invoices",
            column: "InvoiceNumber",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Invoices_InvoiceNumber",
            table: "Invoices");

        migrationBuilder.DropColumn(
            name: "LastInvoiceNo",
            table: "Subhaulers");
    }
}
