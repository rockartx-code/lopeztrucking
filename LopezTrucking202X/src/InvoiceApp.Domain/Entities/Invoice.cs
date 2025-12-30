namespace InvoiceApp.Domain.Entities;

public class Invoice
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateOnly InvoiceDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }
    public List<DetailGroup> DetailGroups { get; set; } = new();
}
