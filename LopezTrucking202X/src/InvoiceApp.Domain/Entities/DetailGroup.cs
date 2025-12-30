namespace InvoiceApp.Domain.Entities;

public class DetailGroup
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Description { get; set; } = string.Empty;
    public decimal AmountBase { get; set; }
    public int EmptiesCount { get; set; }
    public decimal EmptyUnitPrice { get; private set; }

    public decimal AmountTotal => AmountBase + (EmptiesCount * EmptyUnitPrice);

    public void SnapshotEmptyUnitPrice(decimal emptyUnitPrice)
    {
        EmptyUnitPrice = emptyUnitPrice;
    }
}
