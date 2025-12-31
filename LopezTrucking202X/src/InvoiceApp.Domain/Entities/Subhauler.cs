namespace InvoiceApp.Domain.Entities;

public class Subhauler
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? ContactName { get; set; }
    public string? Phone { get; set; }
    public int? LastInvoiceNo { get; set; }
}
