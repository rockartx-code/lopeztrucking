using InvoiceApp.Domain.Entities;

namespace InvoiceApp.Infrastructure.Persistence.Entities;

public class DetailGroupToPlace
{
    public Guid DetailGroupId { get; set; }
    public DetailGroup? DetailGroup { get; set; }
    public Guid PlaceId { get; set; }
    public Place? Place { get; set; }
}
