using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvoiceApp.Infrastructure.Persistence.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Places",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                AddressLine1 = table.Column<string>(type: "TEXT", nullable: false),
                AddressLine2 = table.Column<string>(type: "TEXT", nullable: true),
                City = table.Column<string>(type: "TEXT", nullable: false),
                State = table.Column<string>(type: "TEXT", nullable: false),
                PostalCode = table.Column<string>(type: "TEXT", nullable: false),
                IsCompany = table.Column<bool>(type: "INTEGER", nullable: false),
                IsFrom = table.Column<bool>(type: "INTEGER", nullable: false),
                IsTo = table.Column<bool>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Places", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Settings",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Key = table.Column<string>(type: "TEXT", nullable: false),
                Value = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Settings", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Subhaulers",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                ContactName = table.Column<string>(type: "TEXT", nullable: true),
                Phone = table.Column<string>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Subhaulers", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Companies",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Name = table.Column<string>(type: "TEXT", nullable: false),
                AddressId = table.Column<Guid>(type: "TEXT", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Companies", x => x.Id);
                table.ForeignKey(
                    name: "FK_Companies_Places_AddressId",
                    column: x => x.AddressId,
                    principalTable: "Places",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "CompanyPlaces",
            columns: table => new
            {
                CompanyId = table.Column<Guid>(type: "TEXT", nullable: false),
                PlaceId = table.Column<Guid>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CompanyPlaces", x => new { x.CompanyId, x.PlaceId });
                table.ForeignKey(
                    name: "FK_CompanyPlaces_Companies_CompanyId",
                    column: x => x.CompanyId,
                    principalTable: "Companies",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_CompanyPlaces_Places_PlaceId",
                    column: x => x.PlaceId,
                    principalTable: "Places",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Invoices",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                InvoiceNumber = table.Column<string>(type: "TEXT", nullable: false),
                InvoiceDate = table.Column<string>(type: "TEXT", nullable: false),
                CompanyId = table.Column<Guid>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Invoices", x => x.Id);
                table.ForeignKey(
                    name: "FK_Invoices_Companies_CompanyId",
                    column: x => x.CompanyId,
                    principalTable: "Companies",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "PriceAgreements",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                CompanyId = table.Column<Guid>(type: "TEXT", nullable: false),
                MixName = table.Column<string>(type: "TEXT", nullable: false),
                EffectiveDate = table.Column<string>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PriceAgreements", x => x.Id);
                table.ForeignKey(
                    name: "FK_PriceAgreements_Companies_CompanyId",
                    column: x => x.CompanyId,
                    principalTable: "Companies",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "DetailGroups",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                Description = table.Column<string>(type: "TEXT", nullable: false),
                AmountBase = table.Column<decimal>(type: "TEXT", nullable: false),
                EmptiesCount = table.Column<int>(type: "INTEGER", nullable: false),
                EmptyUnitPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                InvoiceId = table.Column<Guid>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DetailGroups", x => x.Id);
                table.ForeignKey(
                    name: "FK_DetailGroups_Invoices_InvoiceId",
                    column: x => x.InvoiceId,
                    principalTable: "Invoices",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "PriceAgreementItems",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                PriceAgreementId = table.Column<Guid>(type: "TEXT", nullable: false),
                ItemType = table.Column<int>(type: "INTEGER", nullable: false),
                EmptyUnitPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                BaseRate = table.Column<decimal>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PriceAgreementItems", x => x.Id);
                table.ForeignKey(
                    name: "FK_PriceAgreementItems_PriceAgreements_PriceAgreementId",
                    column: x => x.PriceAgreementId,
                    principalTable: "PriceAgreements",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "DetailGroupCompanies",
            columns: table => new
            {
                DetailGroupId = table.Column<Guid>(type: "TEXT", nullable: false),
                CompanyId = table.Column<Guid>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DetailGroupCompanies", x => new { x.DetailGroupId, x.CompanyId });
                table.ForeignKey(
                    name: "FK_DetailGroupCompanies_Companies_CompanyId",
                    column: x => x.CompanyId,
                    principalTable: "Companies",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_DetailGroupCompanies_DetailGroups_DetailGroupId",
                    column: x => x.DetailGroupId,
                    principalTable: "DetailGroups",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "DetailGroupFromPlaces",
            columns: table => new
            {
                DetailGroupId = table.Column<Guid>(type: "TEXT", nullable: false),
                PlaceId = table.Column<Guid>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DetailGroupFromPlaces", x => new { x.DetailGroupId, x.PlaceId });
                table.ForeignKey(
                    name: "FK_DetailGroupFromPlaces_DetailGroups_DetailGroupId",
                    column: x => x.DetailGroupId,
                    principalTable: "DetailGroups",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_DetailGroupFromPlaces_Places_PlaceId",
                    column: x => x.PlaceId,
                    principalTable: "Places",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "DetailGroupToPlaces",
            columns: table => new
            {
                DetailGroupId = table.Column<Guid>(type: "TEXT", nullable: false),
                PlaceId = table.Column<Guid>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DetailGroupToPlaces", x => new { x.DetailGroupId, x.PlaceId });
                table.ForeignKey(
                    name: "FK_DetailGroupToPlaces_DetailGroups_DetailGroupId",
                    column: x => x.DetailGroupId,
                    principalTable: "DetailGroups",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_DetailGroupToPlaces_Places_PlaceId",
                    column: x => x.PlaceId,
                    principalTable: "Places",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Companies_AddressId",
            table: "Companies",
            column: "AddressId");

        migrationBuilder.CreateIndex(
            name: "IX_CompanyPlaces_PlaceId",
            table: "CompanyPlaces",
            column: "PlaceId");

        migrationBuilder.CreateIndex(
            name: "IX_DetailGroupCompanies_CompanyId",
            table: "DetailGroupCompanies",
            column: "CompanyId");

        migrationBuilder.CreateIndex(
            name: "IX_DetailGroupFromPlaces_PlaceId",
            table: "DetailGroupFromPlaces",
            column: "PlaceId");

        migrationBuilder.CreateIndex(
            name: "IX_DetailGroupToPlaces_PlaceId",
            table: "DetailGroupToPlaces",
            column: "PlaceId");

        migrationBuilder.CreateIndex(
            name: "IX_DetailGroups_InvoiceId",
            table: "DetailGroups",
            column: "InvoiceId");

        migrationBuilder.CreateIndex(
            name: "IX_Invoices_CompanyId",
            table: "Invoices",
            column: "CompanyId");

        migrationBuilder.CreateIndex(
            name: "IX_PriceAgreementItems_PriceAgreementId",
            table: "PriceAgreementItems",
            column: "PriceAgreementId");

        migrationBuilder.CreateIndex(
            name: "IX_PriceAgreements_CompanyId",
            table: "PriceAgreements",
            column: "CompanyId");

        migrationBuilder.Sql(
            """
            CREATE VIEW vw_DetailGroups_LegacyShape AS
            SELECT
                dg.Id AS DetailGroupId,
                dg.Description,
                dg.AmountBase,
                dg.EmptiesCount,
                dg.EmptyUnitPrice,
                (dg.AmountBase + (dg.EmptiesCount * dg.EmptyUnitPrice)) AS AmountTotal,
                i.Id AS InvoiceId,
                i.InvoiceNumber,
                i.InvoiceDate,
                i.CompanyId
            FROM DetailGroups dg
            INNER JOIN Invoices i ON i.Id = dg.InvoiceId;
            """);

        migrationBuilder.Sql(
            """
            CREATE VIEW vw_PriceAgreements_LegacyShape AS
            SELECT
                pa.Id AS PriceAgreementId,
                pa.CompanyId,
                pa.MixName,
                pa.EffectiveDate,
                pai.ItemType,
                pai.EmptyUnitPrice,
                pai.BaseRate
            FROM PriceAgreements pa
            INNER JOIN PriceAgreementItems pai ON pai.PriceAgreementId = pa.Id;
            """);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DROP VIEW IF EXISTS vw_PriceAgreements_LegacyShape;");
        migrationBuilder.Sql("DROP VIEW IF EXISTS vw_DetailGroups_LegacyShape;");

        migrationBuilder.DropTable(
            name: "CompanyPlaces");

        migrationBuilder.DropTable(
            name: "DetailGroupCompanies");

        migrationBuilder.DropTable(
            name: "DetailGroupFromPlaces");

        migrationBuilder.DropTable(
            name: "DetailGroupToPlaces");

        migrationBuilder.DropTable(
            name: "PriceAgreementItems");

        migrationBuilder.DropTable(
            name: "Settings");

        migrationBuilder.DropTable(
            name: "Subhaulers");

        migrationBuilder.DropTable(
            name: "DetailGroups");

        migrationBuilder.DropTable(
            name: "PriceAgreements");

        migrationBuilder.DropTable(
            name: "Invoices");

        migrationBuilder.DropTable(
            name: "Companies");

        migrationBuilder.DropTable(
            name: "Places");
    }
}
