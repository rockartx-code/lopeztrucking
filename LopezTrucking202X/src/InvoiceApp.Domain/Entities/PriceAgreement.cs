namespace InvoiceApp.Domain.Entities;

public class PriceAgreement
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }
    public string MixName { get; set; } = string.Empty;
    public string FingerprintText { get; set; } = string.Empty;
    public string FingerprintHash { get; set; } = string.Empty;
    public DateOnly EffectiveDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public bool IsActive { get; set; } = true;
    public List<PriceAgreementItem> Items { get; set; } = new();
}
