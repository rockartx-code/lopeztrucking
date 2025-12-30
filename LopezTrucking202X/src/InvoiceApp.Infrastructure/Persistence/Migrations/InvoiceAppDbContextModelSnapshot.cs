using InvoiceApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace InvoiceApp.Infrastructure.Persistence.Migrations;

public partial class InvoiceAppDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        var dateOnlyConverter = new ValueConverter<DateOnly, string>(
            dateOnly => dateOnly.ToString("yyyy-MM-dd"),
            value => DateOnly.Parse(value));

        modelBuilder
            .HasAnnotation("ProductVersion", "8.0.8");

        modelBuilder.Entity("InvoiceApp.Domain.Entities.Company", b =>
        {
            b.Property<Guid>("Id")
                .HasColumnType("TEXT");

            b.Property<Guid?>("AddressId")
                .HasColumnType("TEXT");

            b.Property<string>("Name")
                .IsRequired()
                .HasColumnType("TEXT");

            b.HasKey("Id");

            b.HasIndex("AddressId");

            b.ToTable("Companies");
        });

        modelBuilder.Entity("InvoiceApp.Domain.Entities.DetailGroup", b =>
        {
            b.Property<Guid>("Id")
                .HasColumnType("TEXT");

            b.Property<decimal>("AmountBase")
                .HasColumnType("TEXT");

            b.Property<string>("Description")
                .IsRequired()
                .HasColumnType("TEXT");

            b.Property<int>("EmptiesCount")
                .HasColumnType("INTEGER");

            b.Property<decimal>("EmptyUnitPrice")
                .HasColumnType("TEXT");

            b.Property<Guid>("InvoiceId")
                .HasColumnType("TEXT");

            b.HasKey("Id");

            b.HasIndex("InvoiceId");

            b.ToTable("DetailGroups");
        });

        modelBuilder.Entity("InvoiceApp.Domain.Entities.Invoice", b =>
        {
            b.Property<Guid>("Id")
                .HasColumnType("TEXT");

            b.Property<Guid>("CompanyId")
                .HasColumnType("TEXT");

            b.Property<DateOnly>("InvoiceDate")
                .HasColumnType("TEXT")
                .HasConversion(dateOnlyConverter);

            b.Property<string>("InvoiceNumber")
                .IsRequired()
                .HasColumnType("TEXT");

            b.HasKey("Id");

            b.HasIndex("CompanyId");

            b.ToTable("Invoices");
        });

        modelBuilder.Entity("InvoiceApp.Domain.Entities.Place", b =>
        {
            b.Property<Guid>("Id")
                .HasColumnType("TEXT");

            b.Property<string>("AddressLine1")
                .IsRequired()
                .HasColumnType("TEXT");

            b.Property<string>("AddressLine2")
                .HasColumnType("TEXT");

            b.Property<string>("City")
                .IsRequired()
                .HasColumnType("TEXT");

            b.Property<bool>("IsCompany")
                .HasColumnType("INTEGER");

            b.Property<bool>("IsFrom")
                .HasColumnType("INTEGER");

            b.Property<bool>("IsTo")
                .HasColumnType("INTEGER");

            b.Property<string>("Name")
                .IsRequired()
                .HasColumnType("TEXT");

            b.Property<string>("PostalCode")
                .IsRequired()
                .HasColumnType("TEXT");

            b.Property<string>("State")
                .IsRequired()
                .HasColumnType("TEXT");

            b.HasKey("Id");

            b.ToTable("Places");
        });

        modelBuilder.Entity("InvoiceApp.Domain.Entities.PriceAgreement", b =>
        {
            b.Property<Guid>("Id")
                .HasColumnType("TEXT");

            b.Property<Guid>("CompanyId")
                .HasColumnType("TEXT");

            b.Property<DateOnly>("EffectiveDate")
                .HasColumnType("TEXT")
                .HasConversion(dateOnlyConverter);

            b.Property<string>("MixName")
                .IsRequired()
                .HasColumnType("TEXT");

            b.HasKey("Id");

            b.HasIndex("CompanyId");

            b.ToTable("PriceAgreements");
        });

        modelBuilder.Entity("InvoiceApp.Domain.Entities.PriceAgreementItem", b =>
        {
            b.Property<Guid>("Id")
                .HasColumnType("TEXT");

            b.Property<decimal>("BaseRate")
                .HasColumnType("TEXT");

            b.Property<decimal>("EmptyUnitPrice")
                .HasColumnType("TEXT");

            b.Property<int>("ItemType")
                .HasColumnType("INTEGER");

            b.Property<Guid>("PriceAgreementId")
                .HasColumnType("TEXT");

            b.HasKey("Id");

            b.HasIndex("PriceAgreementId");

            b.ToTable("PriceAgreementItems");
        });

        modelBuilder.Entity("InvoiceApp.Domain.Entities.Setting", b =>
        {
            b.Property<Guid>("Id")
                .HasColumnType("TEXT");

            b.Property<string>("Key")
                .IsRequired()
                .HasColumnType("TEXT");

            b.Property<string>("Value")
                .IsRequired()
                .HasColumnType("TEXT");

            b.HasKey("Id");

            b.ToTable("Settings");
        });

        modelBuilder.Entity("InvoiceApp.Domain.Entities.Subhauler", b =>
        {
            b.Property<Guid>("Id")
                .HasColumnType("TEXT");

            b.Property<string>("ContactName")
                .HasColumnType("TEXT");

            b.Property<string>("Name")
                .IsRequired()
                .HasColumnType("TEXT");

            b.Property<string>("Phone")
                .HasColumnType("TEXT");

            b.HasKey("Id");

            b.ToTable("Subhaulers");
        });

        modelBuilder.Entity("InvoiceApp.Infrastructure.Persistence.Entities.CompanyPlace", b =>
        {
            b.Property<Guid>("CompanyId")
                .HasColumnType("TEXT");

            b.Property<Guid>("PlaceId")
                .HasColumnType("TEXT");

            b.HasKey("CompanyId", "PlaceId");

            b.HasIndex("PlaceId");

            b.ToTable("CompanyPlaces");
        });

        modelBuilder.Entity("InvoiceApp.Infrastructure.Persistence.Entities.DetailGroupCompany", b =>
        {
            b.Property<Guid>("DetailGroupId")
                .HasColumnType("TEXT");

            b.Property<Guid>("CompanyId")
                .HasColumnType("TEXT");

            b.HasKey("DetailGroupId", "CompanyId");

            b.HasIndex("CompanyId");

            b.ToTable("DetailGroupCompanies");
        });

        modelBuilder.Entity("InvoiceApp.Infrastructure.Persistence.Entities.DetailGroupFromPlace", b =>
        {
            b.Property<Guid>("DetailGroupId")
                .HasColumnType("TEXT");

            b.Property<Guid>("PlaceId")
                .HasColumnType("TEXT");

            b.HasKey("DetailGroupId", "PlaceId");

            b.HasIndex("PlaceId");

            b.ToTable("DetailGroupFromPlaces");
        });

        modelBuilder.Entity("InvoiceApp.Infrastructure.Persistence.Entities.DetailGroupToPlace", b =>
        {
            b.Property<Guid>("DetailGroupId")
                .HasColumnType("TEXT");

            b.Property<Guid>("PlaceId")
                .HasColumnType("TEXT");

            b.HasKey("DetailGroupId", "PlaceId");

            b.HasIndex("PlaceId");

            b.ToTable("DetailGroupToPlaces");
        });

        modelBuilder.Entity("InvoiceApp.Domain.Entities.Company", b =>
        {
            b.HasOne("InvoiceApp.Domain.Entities.Place", "Address")
                .WithMany()
                .HasForeignKey("AddressId");

            b.Navigation("Address");
        });

        modelBuilder.Entity("InvoiceApp.Domain.Entities.DetailGroup", b =>
        {
            b.HasOne("InvoiceApp.Domain.Entities.Invoice", null)
                .WithMany("DetailGroups")
                .HasForeignKey("InvoiceId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity("InvoiceApp.Domain.Entities.Invoice", b =>
        {
            b.HasOne("InvoiceApp.Domain.Entities.Company", "Company")
                .WithMany()
                .HasForeignKey("CompanyId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            b.Navigation("Company");
        });

        modelBuilder.Entity("InvoiceApp.Domain.Entities.PriceAgreement", b =>
        {
            b.HasOne("InvoiceApp.Domain.Entities.Company", "Company")
                .WithMany()
                .HasForeignKey("CompanyId")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            b.Navigation("Company");
        });

        modelBuilder.Entity("InvoiceApp.Domain.Entities.PriceAgreementItem", b =>
        {
            b.HasOne("InvoiceApp.Domain.Entities.PriceAgreement", "PriceAgreement")
                .WithMany("Items")
                .HasForeignKey("PriceAgreementId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("PriceAgreement");
        });

        modelBuilder.Entity("InvoiceApp.Infrastructure.Persistence.Entities.CompanyPlace", b =>
        {
            b.HasOne("InvoiceApp.Domain.Entities.Company", "Company")
                .WithMany()
                .HasForeignKey("CompanyId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.HasOne("InvoiceApp.Domain.Entities.Place", "Place")
                .WithMany()
                .HasForeignKey("PlaceId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("Company");
            b.Navigation("Place");
        });

        modelBuilder.Entity("InvoiceApp.Infrastructure.Persistence.Entities.DetailGroupCompany", b =>
        {
            b.HasOne("InvoiceApp.Domain.Entities.Company", "Company")
                .WithMany()
                .HasForeignKey("CompanyId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.HasOne("InvoiceApp.Domain.Entities.DetailGroup", "DetailGroup")
                .WithMany()
                .HasForeignKey("DetailGroupId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("Company");
            b.Navigation("DetailGroup");
        });

        modelBuilder.Entity("InvoiceApp.Infrastructure.Persistence.Entities.DetailGroupFromPlace", b =>
        {
            b.HasOne("InvoiceApp.Domain.Entities.DetailGroup", "DetailGroup")
                .WithMany()
                .HasForeignKey("DetailGroupId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.HasOne("InvoiceApp.Domain.Entities.Place", "Place")
                .WithMany()
                .HasForeignKey("PlaceId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("DetailGroup");
            b.Navigation("Place");
        });

        modelBuilder.Entity("InvoiceApp.Infrastructure.Persistence.Entities.DetailGroupToPlace", b =>
        {
            b.HasOne("InvoiceApp.Domain.Entities.DetailGroup", "DetailGroup")
                .WithMany()
                .HasForeignKey("DetailGroupId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.HasOne("InvoiceApp.Domain.Entities.Place", "Place")
                .WithMany()
                .HasForeignKey("PlaceId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("DetailGroup");
            b.Navigation("Place");
        });
    }
}
