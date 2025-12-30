using InvoiceApp.Domain.Entities;

namespace InvoiceApp.Infrastructure.Persistence.Entities;

public class CompanyPlace
{
    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }
    public Guid PlaceId { get; set; }
    public Place? Place { get; set; }
}
