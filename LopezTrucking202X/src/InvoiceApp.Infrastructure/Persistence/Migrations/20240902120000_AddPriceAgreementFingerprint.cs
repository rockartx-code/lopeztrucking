using Microsoft.EntityFrameworkCore.Migrations;

namespace InvoiceApp.Infrastructure.Persistence.Migrations;

public partial class AddPriceAgreementFingerprint : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "FingerprintHash",
            table: "PriceAgreements",
            type: "TEXT",
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<string>(
            name: "FingerprintText",
            table: "PriceAgreements",
            type: "TEXT",
            nullable: false,
            defaultValue: string.Empty);

        migrationBuilder.AddColumn<bool>(
            name: "IsActive",
            table: "PriceAgreements",
            type: "INTEGER",
            nullable: false,
            defaultValue: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "FingerprintHash",
            table: "PriceAgreements");

        migrationBuilder.DropColumn(
            name: "FingerprintText",
            table: "PriceAgreements");

        migrationBuilder.DropColumn(
            name: "IsActive",
            table: "PriceAgreements");
    }
}
