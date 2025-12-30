using InvoiceApp.Domain.Entities;

namespace InvoiceApp.Infrastructure.Persistence.Entities;

public class DetailGroupCompany
{
    public Guid DetailGroupId { get; set; }
    public DetailGroup? DetailGroup { get; set; }
    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }
}
