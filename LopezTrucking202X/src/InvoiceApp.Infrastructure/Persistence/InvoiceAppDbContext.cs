using InvoiceApp.Domain.Entities;
using InvoiceApp.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InvoiceApp.Infrastructure.Persistence;

public class InvoiceAppDbContext : DbContext
{
    public InvoiceAppDbContext(DbContextOptions<InvoiceAppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Place> Places => Set<Place>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<DetailGroup> DetailGroups => Set<DetailGroup>();
    public DbSet<PriceAgreement> PriceAgreements => Set<PriceAgreement>();
    public DbSet<PriceAgreementItem> PriceAgreementItems => Set<PriceAgreementItem>();
    public DbSet<Subhauler> Subhaulers => Set<Subhauler>();
    public DbSet<Setting> Settings => Set<Setting>();
    public DbSet<DetailGroupCompany> DetailGroupCompanies => Set<DetailGroupCompany>();
    public DbSet<DetailGroupFromPlace> DetailGroupFromPlaces => Set<DetailGroupFromPlace>();
    public DbSet<DetailGroupToPlace> DetailGroupToPlaces => Set<DetailGroupToPlace>();
    public DbSet<CompanyPlace> CompanyPlaces => Set<CompanyPlace>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var dateOnlyConverter = new ValueConverter<DateOnly, string>(
            dateOnly => dateOnly.ToString("yyyy-MM-dd"),
            value => DateOnly.Parse(value));

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Companies");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.HasOne(e => e.Address)
                .WithMany()
                .HasForeignKey("AddressId");
        });

        modelBuilder.Entity<Place>(entity =>
        {
            entity.ToTable("Places");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.AddressLine1).IsRequired();
            entity.Property(e => e.City).IsRequired();
            entity.Property(e => e.State).IsRequired();
            entity.Property(e => e.PostalCode).IsRequired();
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.ToTable("Invoices");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.InvoiceNumber).IsRequired();
            entity.Property(e => e.InvoiceDate).HasConversion(dateOnlyConverter);
            entity.HasIndex(e => e.InvoiceNumber).IsUnique();
            entity.HasOne(e => e.Company)
                .WithMany()
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(e => e.DetailGroups)
                .WithOne()
                .HasForeignKey("InvoiceId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DetailGroup>(entity =>
        {
            entity.ToTable("DetailGroups");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).IsRequired();
            entity.Property<Guid>("InvoiceId");
            entity.Property(e => e.AmountBase).HasPrecision(18, 2);
            entity.Property(e => e.EmptyUnitPrice).HasPrecision(18, 2);
        });

        modelBuilder.Entity<PriceAgreement>(entity =>
        {
            entity.ToTable("PriceAgreements");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.MixName).IsRequired();
            entity.Property(e => e.FingerprintText).IsRequired();
            entity.Property(e => e.FingerprintHash).IsRequired();
            entity.Property(e => e.EffectiveDate).HasConversion(dateOnlyConverter);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.HasOne(e => e.Company)
                .WithMany()
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(e => e.Items)
                .WithOne(e => e.PriceAgreement)
                .HasForeignKey(e => e.PriceAgreementId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PriceAgreementItem>(entity =>
        {
            entity.ToTable("PriceAgreementItems");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.EmptyUnitPrice).HasPrecision(18, 2);
            entity.Property(e => e.BaseRate).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Subhauler>(entity =>
        {
            entity.ToTable("Subhaulers");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.LastInvoiceNo);
        });

        modelBuilder.Entity<Setting>(entity =>
        {
            entity.ToTable("Settings");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Key).IsRequired();
            entity.Property(e => e.Value).IsRequired();
        });

        modelBuilder.Entity<DetailGroupCompany>(entity =>
        {
            entity.ToTable("DetailGroupCompanies");
            entity.HasKey(e => new { e.DetailGroupId, e.CompanyId });
            entity.HasOne(e => e.DetailGroup)
                .WithMany()
                .HasForeignKey(e => e.DetailGroupId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Company)
                .WithMany()
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DetailGroupFromPlace>(entity =>
        {
            entity.ToTable("DetailGroupFromPlaces");
            entity.HasKey(e => new { e.DetailGroupId, e.PlaceId });
            entity.HasOne(e => e.DetailGroup)
                .WithMany()
                .HasForeignKey(e => e.DetailGroupId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Place)
                .WithMany()
                .HasForeignKey(e => e.PlaceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DetailGroupToPlace>(entity =>
        {
            entity.ToTable("DetailGroupToPlaces");
            entity.HasKey(e => new { e.DetailGroupId, e.PlaceId });
            entity.HasOne(e => e.DetailGroup)
                .WithMany()
                .HasForeignKey(e => e.DetailGroupId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Place)
                .WithMany()
                .HasForeignKey(e => e.PlaceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CompanyPlace>(entity =>
        {
            entity.ToTable("CompanyPlaces");
            entity.HasKey(e => new { e.CompanyId, e.PlaceId });
            entity.Property(e => e.SortOrder).HasDefaultValue(0);
            entity.HasOne(e => e.Company)
                .WithMany()
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Place)
                .WithMany()
                .HasForeignKey(e => e.PlaceId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
