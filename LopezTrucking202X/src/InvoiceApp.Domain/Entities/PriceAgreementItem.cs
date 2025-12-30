namespace InvoiceApp.Domain.Entities;

public class PriceAgreementItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PriceAgreementId { get; set; }
    public PriceAgreement? PriceAgreement { get; set; }
    public ItemType ItemType { get; set; }
    public decimal EmptyUnitPrice { get; set; }
    public decimal BaseRate { get; set; }
}
