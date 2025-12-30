namespace InvoiceApp.Domain.Entities;

public class PriceAgreement
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }
    public DateOnly EffectiveDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public List<PriceAgreementItem> Items { get; set; } = new();
}
